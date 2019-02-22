# Transformations

Transform coordinates between various coordinate systems used in Bulgaria.

## Contents

- [Info](#info)
- [How to use](#how-to-use)
- [API](#api)
  - [Transformations methods](#transformations-methods)
  - [GeoPoint](#geopoint)
  - [GeoExtent](#geoextent)
  - [Ellipsoids](#ellipsoids)
  - [Projections](#projections)
- [Examples](#examples)
  - [Transform between geographic coordinates and projected in Lambert projection](#transform-between-geographic-coordinates-and-projected-in-lambert-projection)
  - [Transform between geographic coordinates and projected in UTM](#transform-between-geographic-coordinates-and-projected-in-utm)
  - [Transform between geographic coordinates and projected in Gauss–Krüger](#transform-between-geographic-coordinates-and-projected-in-gauss-krüger)
  - [Transform between geographic coordinates and projected in Web Mercator](#transform-between-geographic-coordinates-and-projected-in-web-mercator)
  - [Transform between geographic and geocentric coordinates](#transform-between-geographic-and-geocentric-coordinates)
  - [Transform between BGS coordinates](#transform-between-bgs-coordinates)
  - [Format decimal degrees from/to degrees, minutes and seconds](#format-decimal-degrees-from/to-degrees,-minutes-and-seconds)
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

Transformations between BGS coordinate systems is done by calculating transformation parameters for affine transformation based on predefined control points ([ControlPoints namespace](https://github.com/bojko108/Transformations.NET/tree/master/Transformations/ControlPoints)). All other transformations are done directly as the transformation parameters are known. Precision is arround **`20cm`**.

## How to use

You can install it with Nuget:

```
Install-Package Transformations.NET
```

```csharp
using BojkoSoft.Transformations;

Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToUTM(input);
```

## API

### Transformations methods

- Transform geographic coordinates to projected in Lambert projection

```csharp
public GeoPoint TransformGeographicToLambert(GeoPoint inputPoint, enumProjection outputProjection = enumProjection.BGS_2005_KK, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
```

- Transform projected coordinates in Lambert projection to geographic

```csharp
public GeoPoint TransformLambertToGeographic(GeoPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_2005_KK, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
```

- Transform geographic coordinates to projected in UTM

```csharp
public GeoPoint TransformGeographicToUTM(GeoPoint inputPoint, enumProjection outputUtmProjection = enumProjection.UTM35N, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
```

- Transform projected coordinates in UTM to geographic

```csharp
public GeoPoint TransformUTMToGeographic(GeoPoint inputPoint, enumProjection inputUtmProjection = enumProjection.UTM35N, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
```

- Transform geographic coordinates to projected in Gauss–Krüger

```csharp
public GeoPoint TransformGeographicToGauss(GeoPoint inputPoint, enumProjection outputProjection = enumProjection.BGS_1930_24, enumEllipsoid inputEllipsoid = enumEllipsoid.HAYFORD)
```

- Transform projected coordinates in Gauss–Krüger to geographic

```csharp
public GeoPoint TransformGaussToGeographic(GeoPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_1930_24, enumEllipsoid outputEllipsoid = enumEllipsoid.HAYFORD)
```

- Transform geographic coordinates to projected in Web Mercator

```csharp
public GeoPoint TransformGeographicToWebMercator(GeoPoint inputPoint)
```

- Transform projected coordinates in Web Mercator to geographic

```csharp
public GeoPoint TransformWebMercatorToGeographic(GeoPoint inputPoint)
```

- Transform geographic coordinates to geocentric

```csharp
public GeoPoint TransformGeographicToGeocentric(GeoPoint inputPoint, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
```

- Transform geocentric coordinates to geographic

```csharp
public GeoPoint TransformGeocentricToGeographic(GeoPoint inputPoint, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
```

- Transform projected coordinates between BGS coordinate systems

```csharp
public GeoPoint TransformBGSCoordinates(GeoPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_1970_K9, enumProjection outputProjection = enumProjection.BGS_2005_KK)
```

- Format geographic coordinates from decimal degrees to degrees, minutes and seconds

```csharp
public string ConvertDecimalDegreesToDMS(double DEG)
```

- Format geographic coordinates from degrees, minutes and seconds to decimal degrees

```csharp
public double ConvertDMStoDecimalDegrees(string DMS)
```

### GeoPoint

Represents a point that will be or is transformed. Coordinates hold different values for different coordinate systems:
- X coordinate:
    - Geographic: represents Latitude
    - Lambert: represents Northing
    - UTM: represents Northing
    - Old BGS: represents X
    - Cartesian: y
- Y coordinate:
    - Geographic: represents Longitude
    - Lambert: represents Easting
    - UTM: represents Easting
    - Old BGS: represents Y
    - Cartesian: x
- Z coordinate
    - in geocentric: Z
    - in others: Elevation

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
- BGS_2005_KK - Lambert Conformal Conic with 2SP used by Cadastral Agency
- UTM34N - Universal Transverse Mercator zone 34 North
- UTM35N - Universal Transverse Mercator zone 35 North

## Examples

### Transform between geographic coordinates and projected in Lambert projection

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
GeoPoint result = tr.TransformGeographicToLambert(input);
// result is: 4735953.342, 490177.508

GeoPoint input = new GeoPoint(4735953.349, 490177.508);
GeoPoint result = tr.TransformLambertToGeographic(input);
// result is: 42.7589997, 25.3799991
```

### Transform between geographic coordinates and projected in UTM

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToUTM(input);
// result is: 4702270.178, 314955.869

GeoPoint input = new GeoPoint(4702270.179, 314955.869);
GeoPoint result = tr.TransformUTMToGeographic(input);
// result is: 42.450682, 24.749747
```

### Transform between geographic coordinates and projected in Gauss–Krüger

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.7602978166667, 25.3824052611111);
GeoPoint result = tr.TransformGeographicToGauss(input);
// result is: 4736629.503, 8613154.606

GeoPoint input = new GeoPoint(4736629.503, 8613154.607);
GeoPoint result = tr.TransformGaussToGeographic(input);
// result is: 42.7602978, 25.38240528
```

### Transform between geographic coordinates and projected in Web Mercator

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToWebMercator(input);
// result is: 2755129.233, 5228730.328

GeoPoint input = new GeoPoint(2755129.23, 5228730.33);
GeoPoint result = tr.TransformWebMercatorToGeographic(input);
// result is: 42.4506820, 24.7497470
```

### Transform between geographic and geocentric coordinates

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToGeocentric(input);
// result is: X: 4280410.654, 1973273.422, 4282674.061

GeoPoint input = new GeoPoint(4280410.654, 1973273.422, 4282674.061);
GeoPoint result = tr.TransformGeocentricToGeographic(input);
// result is: 42.450682, 24.749747
```

### Transform between BGS coordinates

- BGS 1930

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(4728966.163, 8607005.227);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1930_24, enumProjection.BGS_2005_KK);
// result is: 4728401.432, 483893.508

GeoPoint input = new GeoPoint(4728401.432, 483893.508);
GeoPoint result = tr.TransformBGSCoordinates(input,  enumProjection.BGS_2005_KK, enumProjection.BGS_1930_24);
// result is: 4728966.153, 8607005.214
```

- BGS 1950

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(4729331.175, 8606933.614);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_3_24, enumProjection.BGS_2005_KK);
// result is: 4728401.442, 483893.521

GeoPoint input = new GeoPoint(4728401.432, 483893.508);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_24);
// result is: 4729331.165, 8606933.601
```

- BGS Sofia

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(48276.705, 45420.988);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_SOFIA, enumProjection.BGS_2005_KK);
// result is: 4730215.221, 322402.929

GeoPoint input = new GeoPoint(4730215.229, 322402.935);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_SOFIA);
// result is: 48276.713, 45420.993
```

- BGS 1970

```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(4725270.684, 8515734.475);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK);
// result is: 4816275.688, 332535.346

GeoPoint input = new GeoPoint(4816275.680, 332535.401);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K3);
// result is: 4725270.677, 8515734.530

GeoPoint input = new GeoPoint(4613479.192, 9493233.633);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K5, enumProjection.BGS_2005_KK);
// result is: 4679669.824, 569554.912

GeoPoint input = new GeoPoint(4679669.825, 569554.918);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K5);
// result is: 4613479.193, 9493233.639

GeoPoint input = new GeoPoint(4708089.898, 9570974.988);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K7, enumProjection.BGS_2005_KK);
// result is: 4810276.410, 626498.618

GeoPoint input = new GeoPoint(4810276.431, 626498.611);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K7);
// result is: 4708089.919, 9570974.981

GeoPoint input = new GeoPoint(4547844.976, 8508858.179);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
// result is: 4675440.859, 330568.410

GeoPoint input = new GeoPoint(4675440.847, 330568.434);
GeoPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9);
// result is: 4547844.965, 8508858.203
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

# Dependencies

This project depends on [KDBush](https://github.com/marchello2000/kdbush) project for fast searching in control points classes.

# Tests

Check [Tests](https://github.com/bojko108/Transformations.NET/tree/master/Tests) project for more information.

# License

Transformations is [MIT](https://choosealicense.com/licenses/mit/) License @ bojko108
