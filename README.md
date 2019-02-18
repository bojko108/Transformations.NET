# Transformations

Transform coordinates between various coordinate systems.

## Contents

- [Info](#info)
- [How to use](#how-to-use)
- [API](#api)
    - [Transformations methods](#transformations-methods)
    - [GeoPoint](#geopoint)
    - [Ellipsoids](#ellipsoids)
    - [Projections](#projections)
- [Examples](#examples)
    - [Transform between geographic coordinates and projected in Lambert projection](#transform-between-geographic-coordinates-and-projected-in-lambert-projection)
    - [Transform between geographic coordinates and projected in UTM](#transform-between-geographic-coordinates-and-projected-in-utm)
    - [Transform between geographic coordinates and projected in Gauss–Krüger](#transform-between-geographic-coordinates-and-projected-in-gauss-krüger)
    - [Transform between geographic coordinates and projected in Web Mercator](#transform-between-geographic-coordinates-and-projected-in-web-mercator)
    - [Transform between geographic and geocentric coordinates](#transform-between-geographic-and-geocentric-coordinates)
    - [Transform between BGS 1970 and UTM coordinates](#transform-between-bgs-1970-and-utm-coordinates)
    - [Format decimal degrees from/to degrees, minutes and seconds](#format-decimal-degrees-from/to-degrees,-minutes-and-seconds)
- [Tests](#tests)
- [License](#license)

## Info

The project can be used for transforming coordinates:
- Geographic and Projected coordinates
- Geographic and Geocentric coordinates

Following projections are available:
- Lambert Conformal Conic with 2SP
- Universal Transverse Mercator
- Gauss–Krüger
- BGS 1970
- Spherical Web Mercator

Transformation from/to BGS 1970 is done by calculating transformation parameters for affine transformation based on predefined control points ([ControlPoints.cs](Transformations/ControlPoints/ControlPoints.cs)). All other transformations are done directly as the parameters are known.

## How to use

Add a reference to `Transformations.dll` and:

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
public GeoPoint TransformGeographicToLambert(GeoPoint inputPoint, enumProjections outputProjection = enumProjections.BGS_2005_KK, enumEllipsoids outputEllipsoid = enumEllipsoids.WGS84)
```

- Transform projected coordinates in Lambert projection to geographic

```csharp
public GeoPoint TransformLambertToGeographic(GeoPoint inputPoint, enumProjections inputProjection = enumProjections.BGS_2005_KK, enumEllipsoids inputEllipsoid = enumEllipsoids.WGS84)
```

- Transform geographic coordinates to projected in UTM

```csharp
public GeoPoint TransformGeographicToUTM(GeoPoint inputPoint, enumProjections outputUtmProjection = enumProjections.UTM35N, enumEllipsoids inputEllipsoid = enumEllipsoids.WGS84)
```

- Transform projected coordinates in UTM to geographic

```csharp
public GeoPoint TransformUTMToGeographic(GeoPoint inputPoint, enumProjections inputUtmProjection = enumProjections.UTM35N, enumEllipsoids outputEllipsoid = enumEllipsoids.WGS84)
```

- Transform geographic coordinates to projected in Gauss–Krüger

```csharp
public GeoPoint TransformGeographicToGauss(GeoPoint inputPoint, enumProjections outputProjection = enumProjections.BGS_1930_24, enumEllipsoids inputEllipsoid = enumEllipsoids.HAYFORD)
```

- Transform projected coordinates in Gauss–Krüger to geographic

```csharp
public GeoPoint TransformGaussToGeographic(GeoPoint inputPoint, enumProjections inputProjection = enumProjections.BGS_1930_24, enumEllipsoids outputEllipsoid = enumEllipsoids.HAYFORD)
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
public GeoPoint TransformGeographicToGeocentric(GeoPoint inputPoint, enumEllipsoids outputEllipsoid = enumEllipsoids.WGS84)
```

- Transform geocentric coordinates to geographic

```csharp
public GeoPoint TransformGeocentricToGeographic(GeoPoint inputPoint, enumEllipsoids inputEllipsoid = enumEllipsoids.WGS84)
```

- Transform projected coordinates in BGS 1970 to projected in UTM. All control points are calculated in `UTM35N` projections, so the result coordinates are in that projecton.

```csharp
public GeoPoint Transform1970ToUTM(GeoPoint inputPoint, enumProjections source1970Projection = enumProjections.BGS_1970_К9)
```

- Transform projected coordinates in UTM to projected in BGS KC1970. All control points are calculated in `UTM35N` projections, so the input coordinates must be in that projecton.

```csharp
public GeoPoint TransformUTMTo1970(GeoPoint inputPoint)
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
Represents a point that will be or is transformed. The point has 3 properties `X`, `Y` and `Z`, but they hold different values for different coordinate systems:
- X coordinate:
    - WGS84: represents latitude
    - BGS2005 KK: represents northing
    - UTM: represents northing
    - KC1970: represents x
- Y coordinate:
    - WGS84: represents longitude
    - BGS2005 KK: represents easting
    - UTM: represents easting
    - KC1970: represents y
- Z coordinate

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
- BGS_1970_К3 - ~ Northewest Bulgaria
- BGS_1970_К5 - ~ Southeast Bulgaria
- BGS_1970_К7 - ~ Northeast Bulgaria
- BGS_1970_К9 - ~ Southwest Bulgaria
- BGS_2005_KK - Lambert Conformal Conic with 2SP used by Cadastral agency
- UTM34N - Universal Transverse Mercator zone 34 north
- UTM35N - Universal Transverse Mercator zone 35 north

## Examples

### Transform between geographic coordinates and projected in Lambert projection
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
GeoPoint result = tr.TransformGeographicToLambert(input);
// result is: 4735953.349, 490177.508

GeoPoint input = new GeoPoint(4735953.349, 490177.508);
GeoPoint result = tr.TransformLambertToGeographic(input);
// result is: 42.7589996, 25.3799991
```
### Transform between geographic coordinates and projected in UTM
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToUTM(input);
// result is: 4702270.179, 314955.869

GeoPoint input = new GeoPoint(4702270.179, 314955.869);
GeoPoint result = tr.TransformUTMToGeographic(input);
// result is: 42.450682, 24.749747
```
### Transform between geographic coordinates and projected in Gauss–Krüger
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.7602978166667, 25.3824052611111);
GeoPoint result = tr.TransformGeographicToGauss(input);
// result is: 4736629.503, 8613154.6069

GeoPoint input = new GeoPoint(4736629.503, 8613154.6069);
GeoPoint result = tr.TransformGaussToGeographic(input);
// result is: 42.7602978166667, 25.3824052611111
```
### Transform between geographic coordinates and projected in Web Mercator
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToWebMercator(input);
// result is: 2755129.23, 5228730.33

GeoPoint input = new GeoPoint(2755129.23, 5228730.33);
GeoPoint result = tr.TransformWebMercatorToGeographic(input);
// result is: 42.450682, 24.749747
```
### Transform between geographic and geocentric coordinates
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToGeocentric(input);
// result is: 4280410.654, 1973273.422, 4282674.061

GeoPoint input = new GeoPoint(4280410.654, 1973273.422, 4282674.061);
GeoPoint result = tr.TransformGeocentricToGeographic(input);
// result is: 42.450682, 24.749747
```
### Transform between BGS 1970 and UTM coordinates
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(4577015.806, 8615896.123);
GeoPoint result = tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К9);
// result is: 4702270.179, 314955.869

GeoPoint input = new GeoPoint(4702270.179, 314955.869);
GeoPoint result = tr.TransformUTMTo1970(input);
// result is: 4577015.806, 8615896.123
```
### Format decimal degrees from/to degrees, minutes and seconds
```csharp
Transformations tr = new Transformations();

double latitude = 42.336542;
string dms = tr.ConvertDecimalDegreesToDMS(latitude);
// result is: 422011.5512000000052

string dms = "422011.5512000000052";
double result = tr.ConvertDMStoDecimalDegrees(dms);
// result is: 42.336542
```

# Tests

Check [Tests](https://github.com/bojko108/Transformations.NET/tree/master/Tests) project for more information.

## License

Transformations is [MIT](https://choosealicense.com/licenses/mit/) License @ bojko108
