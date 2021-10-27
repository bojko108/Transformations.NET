# Transformations

Transform coordinates between various coordinate systems used in Bulgaria.

## Contents

- [Info](#info)
- [How to use](#how-to-use)
- [API](#api)
  - [IPoint](#ipoint)
  - [IExtent](#iextent)
  - [Ellipsoids](#ellipsoids)
  - [Projections](#projections)
  - [Transformation methods](#transformation-methods)
- [Examples](#examples)
  - [Transform between geographic coordinates and projected in Lambert projection](#transform-between-geographic-coordinates-and-projected-in-lambert-projection)
  - [Transform between geographic coordinates and projected in UTM](#transform-between-geographic-coordinates-and-projected-in-utm)
  - [Transform between geographic coordinates and projected in Gauss–Krüger](#transform-between-geographic-coordinates-and-projected-in-gauss-krüger)
  - [Transform between geographic coordinates and projected in Web Mercator](#transform-between-geographic-coordinates-and-projected-in-web-mercator)
  - [Transform between geographic and geocentric coordinates](#transform-between-geographic-and-geocentric-coordinates)
  - [Transform between BGS coordinates](#transform-between-bgs-coordinates)
  - [Transform by calculating transformation parameters for an extent](#transform-by-calculating-transformation-parameters-for-an-extent)
  - [Format decimal degrees from/to degrees, minutes and seconds](#format-decimal-degrees-from/to-degrees,-minutes-and-seconds)
- [TODO](#todo)
- [Dependencies](#dependencies)
- [Tests](#tests)
- [License](#license)

## Info

The project can be used for transforming coordinates between various coordinate systems:

- BGS 1930
- BGS 1950
- BGS Sofia
- BGS 1970 (there can be no control points near by if the point is close to the zone's border)
- BGS 2005

Following projections are available:

- Lambert Conformal Conic with 2SP
- Universal Transverse Mercator
- Gauss–Krüger
- Spherical Web Mercator

You can transform between:

- Geographic coordinates
- Projected coordinates
- Geocentric coordinates

Transformations between BGS coordinate systems are done by calculating transformation parameters for Affine or Thin Plate Spline (TPS) transformation based on predefined control points ([ControlPoints namespace](https://github.com/bojko108/Transformations.NET/tree/master/Transformations/ControlPoints)). You can control what type of transformation is used by passing `useTPS` boolean parameter to `TransformBGSCoordinates()`. All other transformations are done directly as the transformation parameters are known.

Optionally you can calculate transformation parameters for affine transformation valid for specific region and use them to calculate points more quickly, but with less precission.

The library is available in JavaScript too: [transformations](http://github.com/bojko108/transformations).

## How to use

1. Install with Nuget:

```
Install-Package Transformations.NET
```

2. Implement [IPoint](#ipoint) and/or [IExtent](#iextent) interfaces:

```csharp
using BojkoSoft.Transformations;
...
class Point : IPoint
{
    public double N { get; set; }
    public double E { get; set; }
    public double Z { get; set; }

    public TestPoint(double x, double y, double z)
    {
        ...
    }

    public IPoint Clone()
        => new Point(this.N, this.E, this.Z);
}

class Extent : IExtent
{
    public double MinN { get; set; }
    public double MinE { get; set; }
    public double MaxN { get; set; }
    public double MaxE { get; set; }
    public double Width => this.MaxE - this.MinE;
    public double Height => this.MaxN - this.MinN;
    public bool IsEmpty => this.Width <= 0 && this.Height <= 0;

    public TestExtent(double northingMax, double northingMin, double eastingMax, double eastingMin)
    {
        ...
    }

    public void Expand(double meters)
    {
        ...
    }

    public IExtent Clone()
        => new TestExtent(this.MaxN, this.MinN, this.MaxE, this.MinE);
}
```

2. Instantiate `Transformations` class. When instatiating the class you can pass a `boolean` parameter whether the control points must be initialized (default is `true` - control points are initialized). If you plan to transform between BGS coordinate systems the contrl points MUST BE initialized otherwisee you wont be able to transform coordinates. If you don't plan to transform between BGS coordinate systems you can pass `false` and the control points wont be added to the memory.

```csharp
using BojkoSoft.Transformations;
...
Transformations tr = new Transformations(/* pass false and control points won't ne initialized */);

IPoint input = new Point(42.450682, 24.749747);
IPoint result = tr.TransformGeographicToUTM(input);
```

A posible solution is to create a singleton class, simmilar to this one:

```csharp
using BojkoSoft.Transformations;
...
namespace YourNamespace
{
  sealed class TransformationUtils
  {
    public static TransformationUtils Instance => _instance;
    public enumProjection SourceProjection { get; set; }
    public enumProjection TargetProjection { get; set; }
    public bool NeedsTransformation
    {
      get
      {
        return this.SourceProjection != CoordinateSystem.Unknown
            && this.TargetProjection != CoordinateSystem.Unknown
            && this.SourceProjection != this.TargetProjection;
      }
    }

    private static readonly TransformationUtils _instance = new TransformationUtils();
    private readonly Transformations tr;
    private double[] parameters;

    private TransformationUtils()
    {
      this.tr = new Transformations();
      this.parameters = null;

      this.SourceProjection = enumProjection.Unknown;
      this.TargetProjection = enumProjection.Unknown;
    }

    public void CalculateTransformationParameters(IExtent extent)
    {
      // ensure that the extent is big enough to include more control points
      // for calculating better transformation parameters
      if (extent.IsEmpty || extent.Height < 20000 || extent.Width < 20000)
        extent.Expand(20000);
      this.parameters = this.tr.CalculateAffineTransformationParameters(
        extent, source, target);
    }

    public IPoint TransformPoint(IPoint sourcePoint)
    {
      IPoint outputPoint = null;
      switch (this.SourceProjection)
      {
        case enumProjection.BGS_1970_K9:
        {
          switch (this.TargetProjection)
          {
            case enumProjection.BGS_2005_KK:
            {
              outputPoint = this.tr.TransformBGSCoordinates(
                sourcePoint, this.parameters,
                (int)this.SourceProjection,
                (int)this.TargetProjection);
              break;
            }
            case enumProjection.WGS84_GEOGRAPHIC:
            {
              outputPoint = this.tr.TransformBGSCoordinates(
                sourcePoint,
                this.parameters,
                (int)this.SourceProjection,
                (int)enumProjection.BGS_2005_KK);
              outputPoint = this.tr.TransformLambertToGeographic(outputPoint);
              break;
            }
          }
          break;
        }
      }
      return outputPoint;
    }
  }
}
```

`TransformationUtils.TransformPoint()` is the main method used for transforming points using the above singleton class. You can implement one or more transformations between `enumProjection` projections in the `switch`.

**Using a singleton class**

```csharp
TransformationUtils.Instance.SourceProjection = enumProjection.BGS_1970_K9;
TransformationUtils.Instance.TargetProjection = enumProjection.WGS84_GEOGRAPHIC;

// calculate transformation parameters if you plan to process more points
TransformationUtils.Instance.CalculateTransformationParameters(new IExtent(...));

Point bgs1970k9 = new Point(4547844.976, 8508858.179);
Point wgs84 = TransformationUtils.Instance.TransformPoint(bgs1970k9);
// result is: 42.195768, 23.448409
```

## API

This library exposes several objects for easier integration. Main workflows for transforming coordinates are:

**Without calculating transformation parameters**

This method can be used if you don't transform between BGS coordinate systems:

```csharp
Transformations tr = new Transformations(false);  // false means no control points will be added to memory
IPoint input = new Point(4735953.349, 490177.508);
IPoint result = tr.TransformLambertToGeographic(input);
```

Or if you plan to transform few points:

```csharp
Transformations tr = new Transformations();
IPoint transformed = tr.TransformBGSCoordinates(inputPoint, BGS_1970_K9, WGS84_GEOGRAPHIC);
```

**With calculating transformation parameters**

If you plan to transform more than few points it is suggested to calculate transformation parameters for the whole area where your coordiantes are located and then use them to calculate all points in the area:

```csharp
IExtent extent = new Extent(...);
Transformations tr = new Transformations();
double[] parameters = tr.CalculateAffineTransformationParameters(extent, ...);
IPoint transformed = tr.TransformBGSCoordinates(inputPoint, parameters, BGS_1970_K9, WGS84_GEOGRAPHIC)
```

This will make the process run faster as the parameters will be calculated only once.

### IPoint

Represents a point geometry, ready for transformation. Coordinates hold different values for different coordinate systems:

- N coordinate:
  - Geographic: represents Latitude
  - Lambert: represents Northing
  - UTM: represents Northing
  - Old BGS: represents X
  - Cartesian: y
- E coordinate:
  - Geographic: represents Longitude
  - Lambert: represents Easting
  - UTM: represents Easting
  - Old BGS: represents Y
  - Cartesian: x
- Z coordinate
  - in geocentric: Z
  - in others: Elevation

### IExtent

Represents an extent. Implement this interface if you transform between BGS coordinate systems and plan to transform more coordinates. You can create an extent for the area where the coordinates are located and then calculate transformation parameters for that area. Make sure the area is big enough to contain more control points - in most cases area with diameter of ~20km is more than enough. This will make the transformation run faster:

```csharp
IExtent extent = new Extent(...);
Transformations tr = new Transformations();
double[] parameters = tr.CalculateAffineTransformationParameters(extent, ...);
IPoint transformed = tr.TransformBGSCoordinates(inputPoint, parameters, ...);
```

### Ellipsoids

```csharp
using BojkoSoft.Transformations.Constants;
```

- GRS80 - GRS 1980
- BESSEL_1841 - Bessel 1841
- CLARKE_1866 - Clarke 1866
- EVEREST - Everest (Sabah and Sarawak)
- HELMERT - Helmert 1906
- HAYFORD - Hayford
- KRASSOVSKY - Krassovsky, 1942
- WALBECK - Walbeck
- WGS72 - WGS 1972
- WGS84 - WGS 1984
- SPHERE - Normal Sphere (R=6378137)

### Projections

```csharp
using BojkoSoft.Transformations.Constants;
```

- WGS84_GEOGRAPHIC (EPSG:4326) - geographic coordinates with WGS84 ellipsoid
- BGS_SOFIA - BGS Sofia. Local projection based on BGS 1950
- BGS_1930_24 - Gauss–Krüger projection based on Hayford ellipsoid
- BGS_1930_27 - Gauss–Krüger projection based on Hayford ellipsoid
- BGS_1950_3_24 - Gauss–Krüger projection based on Krassovsky ellipsoid
- BGS_1950_3_27 - Gauss–Krüger projection based on Krassovsky ellipsoid
- BGS_1950_6_21 - Gauss–Krüger projection based on Krassovsky ellipsoid
- BGS_1950_6_27 - Gauss–Krüger projection based on Krassovsky ellipsoid
- BGS_1970_K3 - ~ Northewest Bulgaria
- BGS_1970_K5 - ~ Southeast Bulgaria
- BGS_1970_K7 - ~ Northeast Bulgaria
- BGS_1970_K9 - ~ Southwest Bulgaria
- BGS_2005_KK (EPSG:7801) - Lambert Conformal Conic with 2SP used by Cadastral Agency
- UTM34N (EPSG:32634) - Universal Transverse Mercator zone 34 North
- UTM35N (EPSG:32635) - Universal Transverse Mercator zone 35 North
- Unknown - unknown projection (coordinate system)

### Transformation methods

- Transform geographic coordinates to projected in Lambert projection

```csharp
public IPoint TransformGeographicToLambert(IPoint inputPoint, enumProjection outputProjection = enumProjection.BGS_2005_KK, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
```

- Transform projected coordinates in Lambert projection to geographic

```csharp
public IPoint TransformLambertToGeographic(IPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_2005_KK, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
```

- Transform geographic coordinates to projected in UTM

```csharp
public IPoint TransformGeographicToUTM(IPoint inputPoint, enumProjection outputUtmProjection = enumProjection.UTM35N, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
```

- Transform projected coordinates in UTM to geographic

```csharp
public IPoint TransformUTMToGeographic(IPoint inputPoint, enumProjection inputUtmProjection = enumProjection.UTM35N, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
```

- Transform geographic coordinates to projected in Gauss–Krüger

```csharp
public IPoint TransformGeographicToGauss(IPoint inputPoint, enumProjection outputProjection = enumProjection.BGS_1930_24, enumEllipsoid inputEllipsoid = enumEllipsoid.HAYFORD)
```

- Transform projected coordinates in Gauss–Krüger to geographic

```csharp
public IPoint TransformGaussToGeographic(IPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_1930_24, enumEllipsoid outputEllipsoid = enumEllipsoid.HAYFORD)
```

- Transform geographic coordinates to projected in Web Mercator

```csharp
public IPoint TransformGeographicToWebMercator(IPoint inputPoint)
```

- Transform projected coordinates in Web Mercator to geographic

```csharp
public IPoint TransformWebMercatorToGeographic(IPoint inputPoint)
```

- Transform geographic coordinates to geocentric

```csharp
public IPoint TransformGeographicToGeocentric(IPoint inputPoint, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
```

- Transform geocentric coordinates to geographic

```csharp
public IPoint TransformGeocentricToGeographic(IPoint inputPoint, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
```

- Transform projected coordinates between BGS coordinate systems

```csharp
public IPoint TransformBGSCoordinates(IPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_1970_K9, enumProjection outputProjection = enumProjection.BGS_2005_KK, bool useTPS = true)
```

- Format geographic coordinates from decimal degrees to degrees, minutes and seconds

```csharp
public string ConvertDecimalDegreesToDMS(double DEG)
```

- Format geographic coordinates from degrees, minutes and seconds to decimal degrees

```csharp
public double ConvertDMStoDecimalDegrees(string DMS)
```

- Calculate Affine transformation parameters for a region

```csharp
public double[] CalculateAffineTransformationParameters(IExtent inputExtent, enumProjection inputProjection = enumProjection.BGS_1970_K9, enumProjection outputProjection = enumProjection.BGS_2005_KK)
```

## Examples

### Transform between geographic coordinates and projected in Lambert projection

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(42.7589996, 25.3799991);
IPoint result = tr.TransformGeographicToLambert(input);
// result is: 4735953.342, 490177.508

IPoint input = new Point(4735953.349, 490177.508);
IPoint result = tr.TransformLambertToGeographic(input);
// result is: 42.7589997, 25.3799991
```

### Transform between geographic coordinates and projected in UTM

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(42.450682, 24.749747);
IPoint result = tr.TransformGeographicToUTM(input);
// result is: 4702270.178, 314955.869

IPoint input = new Point(4702270.179, 314955.869);
IPoint result = tr.TransformUTMToGeographic(input);
// result is: 42.450682, 24.749747
```

### Transform between geographic coordinates and projected in Gauss–Krüger

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(42.7602978166667, 25.3824052611111);
IPoint result = tr.TransformGeographicToGauss(input);
// result is: 4736629.503, 8613154.606

IPoint input = new Point(4736629.503, 8613154.607);
IPoint result = tr.TransformGaussToGeographic(input);
// result is: 42.7602978, 25.38240528
```

### Transform between geographic coordinates and projected in Web Mercator

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(42.450682, 24.749747);
IPoint result = tr.TransformGeographicToWebMercator(input);
// result is: 2755129.233, 5228730.328

IPoint input = new Point(2755129.23, 5228730.33);
IPoint result = tr.TransformWebMercatorToGeographic(input);
// result is: 42.4506820, 24.7497470
```

### Transform between geographic and geocentric coordinates

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(42.450682, 24.749747);
IPoint result = tr.TransformGeographicToGeocentric(input);
// result is: X: 4280410.654, 1973273.422, 4282674.061

IPoint input = new Point(4280410.654, 1973273.422, 4282674.061);
IPoint result = tr.TransformGeocentricToGeographic(input);
// result is: 42.450682, 24.749747
```

### Transform between BGS coordinates

Transformations between BGS coordinate systems are than by calculating transformation parameters using predefined set of control points. Control points are available for following coordinate systems:
`BGS_1930_24`, `BGS_1930_27`, `BGS_1950_3_24`, `BGS_1950_3_27`, `BGS_1950_6_21`, `BGS_1950_6_27`, `BGS_1970_K3`, `BGS_1970_K5`, `BGS_1970_K7`, `BGS_1970_K9`, `BGS_2005_KK`. If you need to transform for example from `BGS_1970_K9` to `WGS84_GEOGRAPHIC` here is the workflow to do so:

```csharp
Transformations tr = new Transformations();
// first transform to BGS_2005_KK using control points
IPoint outputPoint = tr.TransformBGSCoordinates(sourcePoint, BGS_1970_K9, BGS_2005_KK);
// then transform to WGS84_GEOGRAPHIC
outputPoint = this.tr.TransformLambertToGeographic(outputPoint);
```

- BGS 1930

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(4728966.163, 8607005.227);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1930_24, enumProjection.BGS_2005_KK);
// result is: 4728401.432, 483893.508

IPoint input = new Point(4728401.432, 483893.508);
IPoint result = tr.TransformBGSCoordinates(input,  enumProjection.BGS_2005_KK, enumProjection.BGS_1930_24);
// result is: 4728966.153, 8607005.214
```

- BGS 1950

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(4729331.175, 8606933.614);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_3_24, enumProjection.BGS_2005_KK);
// result is: 4728401.442, 483893.521

IPoint input = new Point(4728401.432, 483893.508);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_24);
// result is: 4729331.165, 8606933.601
```

- BGS Sofia

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(48276.705, 45420.988);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_SOFIA, enumProjection.BGS_2005_KK);
// result is: 4730215.221, 322402.929

IPoint input = new Point(4730215.229, 322402.935);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_SOFIA);
// result is: 48276.713, 45420.993
```

- BGS 1970

```csharp
Transformations tr = new Transformations();

IPoint input = new Point(4725270.684, 8515734.475);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK);
// result is: 4816275.688, 332535.346

IPoint input = new Point(4816275.680, 332535.401);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K3);
// result is: 4725270.677, 8515734.530

IPoint input = new Point(4613479.192, 9493233.633);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K5, enumProjection.BGS_2005_KK);
// result is: 4679669.824, 569554.912

IPoint input = new Point(4679669.825, 569554.918);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K5);
// result is: 4613479.193, 9493233.639

IPoint input = new Point(4708089.898, 9570974.988);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K7, enumProjection.BGS_2005_KK);
// result is: 4810276.410, 626498.618

IPoint input = new Point(4810276.431, 626498.611);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K7);
// result is: 4708089.919, 9570974.981

IPoint input = new Point(4547844.976, 8508858.179);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
// result is: 4675440.859, 330568.410

IPoint input = new Point(4675440.847, 330568.434);
IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9);
// result is: 4547844.965, 8508858.203
```

### Transform by calculating transformation parameters for an extent

- You first provide the extent and calculate transformation parameters usi.......................

```csharp
IExtent extent = new Extent(4590706, 4556298, 8561889, 8519105);
// optionaly you can expand the provided extent:
// extent.Expand(20000);

Transformations tr = new Transformations();

// calculate transformation parameters
double[] parameters = tr.CalculateAffineTransformationParameters(extent, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);

// transform input point with calculated parameters
IPoint input = new Point(4573488, 8539465);
IPoint result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
// result is: 4700322.190, 361795.526
```

### Format decimal degrees from/to degrees, minutes and seconds

```csharp
Transformations tr = new Transformations();

double latitude = 42.336542;
string dms = tr.ConvertDecimalDegreesToDMS(latitude);
// result is: "422011.5512000000052"

string dms = "422011.5512000000052";
double result = tr.ConvertDMStoDecimalDegrees(dms);
// result is: 42.336542
```

# TODO

- Transform array of points
- Fix IPoint constructor when used like: `new Point(int id, double X, double Y)`
- Implement Polynomial transformation

# Tests

Check [Tests](https://github.com/bojko108/Transformations.NET/tree/master/Tests) project for more information.

# License

Transformations.NET is [MIT](https://choosealicense.com/licenses/mit/) License @ bojko108
