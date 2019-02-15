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
    - [Transform between Geographic and UTM coordinates](#transform-between-geographic-and-utm-coordinates)
    - [Transform between KC1970 and UTM coordinates](#transform-between-kc1970-and-utm-coordinates)
    - [Transform between BGS 2005 and UTM coordinates](#transform-between-bgs-2005-and-utm-coordinates)
    - [Transform between Web Mercator and Geographic coordinates](#transform-between-web-mercator-and-geographic-coordinates)
    - [Format decimal degrees from/to degrees, minutes and seconds](#format-decimal-degrees-from/to-degrees,-minutes-and-seconds)
- [Tests](#tests)
- [License](#license)

## Info

The project can be used for transforming coordinates between following coordinate systems:

- Geographic (WGS84)
- UTM
- KC1970
- BGS 2005 - Cadastral coordinate system
- Spherical Web Mercator

Transformation from/to KC1970 is done by calculating transformation parameters for affine transformation based on predefined control points ([ControlPoints.cs](Transformations/ControlPoints/ControlPoints.cs)). All other transformations are done directly as the parameters are known.

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

- Transform geographic coordinates (WGS84) to projected (UTM)

```csharp
public GeoPoint TransformGeographicToUTM(GeoPoint inputPoint, enumUtmZone targetUtmZone = enumUtmZone.Zone35N)
```

- Transform projected coordinates (UTM) to geographic (WGS84)

```csharp
public GeoPoint TransformUTMToGeographic(GeoPoint inputPoint, enumUtmZone sourceUtmZone = enumUtmZone.Zone35N)
```

- Transform projected coordinates (KC1970) to projected (UTM)

```csharp
public GeoPoint Transform1970ToUTM(GeoPoint inputPoint, enumKC1970Zone source1970Zone = enumKC1970Zone.K9)
```

- Transform projected coordinates (UTM) to projected (KC1970)

```csharp
public GeoPoint TransformUTMTo1970(GeoPoint inputPoint)
```

- Transform projected coordinates (BGS2005 KK) to geographic (WGS84)

```csharp
public GeoPoint TransformLambertProjectedToGeographic(GeoPoint inputPoint)
```

- Transform geographic coordinates (WGS84) to projected (BGS2005 KK)

```csharp
public GeoPoint TransformGeographicToLambertProjected(GeoPoint inputPoint)
```

- Transform projected coordinates (Web Mercator) to geographic (WGS84)

```csharp
public GeoPoint TransformGeographicToWebMercator(GeoPoint inputPoint)
```

- Transform geographic coordinates (WGS84) to projected (Web Mercator)

```csharp
public GeoPoint TransformWebMercatorToGeographic(GeoPoint inputPoint)
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
Represents a point that will be or is transformed. The point has only 2 properties `X` and `Y`, but they hold different values for different coordinate systems:
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

### Ellipsoids
- WGS84
- Sphere (R=6378137m)

### Projections
- BGS2005 KK - Uses Lambert Conformal Conic projection with 2SP
- UTM34N - UTM zone 34 north
- UTM35N - UTM zone 35 north

## Examples

### Transform between Geographic and UTM coordinates
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToUTM(input, enumUtmZone.Zone35N);
// result is: 4702270.179, 314955.869

GeoPoint input = new GeoPoint(4702270.179, 314955.869);
GeoPoint result = tr.TransformUTMToGeographic(input, enumUtmZone.Zone35N);
// result is: 42.450682, 24.749747
```
### Transform between KC1970 and UTM coordinates
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(4577015.806, 8615896.123);
GeoPoint result = tr.Transform1970ToUTM(input, enumKC1970Zone.K9);
// result is: 4702270.179, 314955.869

GeoPoint input = new GeoPoint(4702270.179, 314955.869);
GeoPoint result = tr.TransformUTMTo1970(input);
// result is: 4577015.806, 8615896.123
```
### Transform between BGS 2005 and UTM coordinates
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
GeoPoint result = tr.TransformGeographicToLambertProjected(input);

GeoPoint input = new GeoPoint(4735953.349, 490177.508);
GeoPoint result = tr.TransformLambertProjectedToGeographic(input);
// result is: 42.7589996, 25.3799991
```
### Transform between Web Mercator and Geographic coordinates
```csharp
Transformations tr = new Transformations();

GeoPoint input = new GeoPoint(42.450682, 24.749747);
GeoPoint result = tr.TransformGeographicToWebMercator(input);
// result is: 2755129.23, 5228730.33

GeoPoint input = new GeoPoint(2755129.23, 5228730.33);
GeoPoint result = tr.TransformWebMercatorToGeographic(input);
// result is: 42.450682, 24.749747
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

Check [Tests](Transformations/Tests) project for more information.

## License

Transformations is [MIT](https://choosealicense.com/licenses/mit/) License @ bojko108
