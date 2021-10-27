using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransUtils
{
    internal enum ellipsoid : ushort      // Референтни елипсоиди
    {
        GRS80 = 0,      // GRS80			// Геодезическата референтна система GRS80 епоха 1980
        WGS84 = 1,      // WGS84			// Световна геодезическа система епоха 2005?
        KRASOVSKI = 2,  // Красовски 1942	// ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1942 г. (ГРС  42)/83, 1950, 1970
        HAYFORD = 3,    // Хейфорд 1910		// БЪЛГАРСКА ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1930 г. (БГРС 1930)
        HELMERT = 4,    // Helmert 1907
        CLARKE1880 = 5, // Clarke 1880	
        CLARKE1866 = 6, // Clarke 1866
        BESSEL = 7,     // Bessel 1841
        EVEREST = 8,    // Everest 1830
        VALBEK = 9      // Валбек 1819		// ГЕОДЕЗИЧЕСКАТА РЕФЕРЕНТНА НА “РУСКАТА” ТРИАНГУЛАЦИЯ НА БЪЛГАРИЯ 1877-1878
    }

    internal enum CoordSys : ushort       // Координатни системи
    {
        // NULL	= 0,	// Няма дефинирана система (планова или височинна)
        CS1930 = 1,     // Гаус-Крюгерова проекция върху елипсоид Хейфорд, изходна точка Черни връх
        CS1950 = 2,     // Гаус-Крюгерова проекция върху елипсоид Красовски, локално за БГ, изходна точка Черни връх
        CS1942 = 3,     // Гаус-Крюгерова проекция върху елипсоид Красовски, изравнена със СССР, Унгария, Полша, ГДР, Чехословакия, Румъния
        CS4283 = 4,     // Гаус-Крюгерова проекция върху елипсоид Красовски, изравнена със СССР, Унгария, Полша, ГДР, Чехословакия, Румъния през 1983
        CS1970 = 5,     // Ламбертова проекция 4 зони - К3, К5, К7 и К9 върху елипсоид Красовски, произлиза от КС1950
        CSSofiiska = 6, // Гаус-Крюгерова проекция върху елипсоид Красовски, редуцирана КС1950 dX = 4680000.000	и dY = 8400000.000

        CS2000 = 7,     // Ламбертова проекция върху елипсоид GRS80, КС2000
        CS2005 = 8,     // UTM проекция върху елипсоид GRS80, епоха 2005, с трансформационни праметри зона 34, 35 за Бългрия
                        // Ламбертова проекция върху елипсоид GRS80, епоха 2005, зона 0
        ETRS89 = 9,     // БулРеф89 GRS80 - WGS84

        UTM = 10,       // UTM проекция върху елипсоид WGS84  -  Напречно цилиндрична Гаусова проекция
        Lambert = 11,   // Ламбертова проекция - Конична конформна с два стандартни паралела
        Gauss = 12      // Гаусова проекция -  Напречно цилиндрична
    };
    // Номенклатура на тип на координатите!
    internal enum CoordType : ushort      // тип на координатните системи
    {
        XYH = 0,        // Проекционни координати и надморска височина (Декартови координати)
        BLH = 1,        // Пространствени Геодезически Географски координати
        XYZ = 2         // Пространствени Геоцентрични координати
    };

    internal class Transformation
    {
        private static double _PI = 3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679e0;
        private static double _2PI = 6.2831853071795864769252867665590057683943387987502116419498891846156328125724179972560696506842341358e0;
        // Екваториален радиус на Земята (голяма полуос) 
        private static double[] Semi_major_axis = {			// a - Голяма полуос на ротационен елипсоид
				6378137, 		// GRS80			// Геодезическата референтна система GRS80 епоха 1980
				6378137, 		// WGS84			// Световна геодезическа система епоха 2005?
				6378245, 		// Красовски 1942	// ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1942 г. (ГРС  42)/83, 1950, 1970
				6378388, 		// Хейфорд 1910		// БЪЛГАРСКА ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1930 г. (БГРС 1930)
			   	6378200, 		// Helmert 1907
				6378249, 		// Clarke 1880
				6378206.400,	// Clarke 1866
				6377397, 		// Bessel 1841
			   	6377276, 		// Everest 1830
				6376896			// Валбек 1819		// ГЕОДЕЗИЧЕСКАТА РЕФЕРЕНТНА НА “РУСКАТА” ТРИАНГУЛАЦИЯ НА БЪЛГАРИЯ 1877-1878 
		};

        // Полярна сплеснатост на Земята
        private static double[] Inverse_flattening = {		// 1/f - Полярна сплеснатост на ротационен елипсоид
				298.257222101,	// GRS80 1980		// Геодезическата референтна система GRS80 епоха 1980
				298.257223563,	// WGS84			// Световна геодезическа система епоха 2005?
				298.3,			// Красовски 1942	// ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1942 г. (ГРС  42)/83, 1950, 1970
				297.0,			// Хейфорд 1910		// БЪЛГАРСКА ГЕОДЕЗИЧЕСКА  РЕФЕРЕНТНА СИСТЕМА 1930 г. (БГРС 1930)
				298.3,			// Helmert 1907
				293.5,			// Clarke 1880
				294.97870,		// Clarke 1866
				299.15,			// Bessel 1841
				301.0,			// Everest 1830
				302.78			// Валбек 1819		// ГЕОДЕЗИЧЕСКАТА РЕФЕРЕНТНА НА “РУСКАТА” ТРИАНГУЛАЦИЯ НА БЪЛГАРИЯ 1877-1878 
		};

        public Transformation() { }                 // Конструктор на класа
        ~Transformation() { }                       // Деструктор на класа;

        // Трансформация между геодезически координатни системи
        public bool PrjTransform(ushort fromCoordSys, ushort fromCoordZone, ushort fromCoordType, ushort fromLevel,
            ushort toCoordSys, ushort toCoordZone, ushort toCoordType, short toCurrentZone, ushort toLevel,
            ref double x_output, ref double y_output, ref double z_output)      // Трансформация м/у геодезически координатни системи
        {
            short currentZone = 0;
            double x_input = x_output;
            double y_input = y_output;
            double z_input = z_output;

            double X_1950, Y_1950, H_1950;      // Проекционни гаусови координати в 6° ивица!
            double X_4283, Y_4283, Z_4283;      // Геоцентрични Декартови координати - елипсоид Красовски 1942/83
            double X_WGS84, Y_WGS84, Z_WGS84;   // Геоцентрични Декартови координати - елипсоид WGS84
            double X_2005, Y_2005, Z_2005;      // Геоцентрични Декартови координати - елипсоид WGS84 епоха 2005
            double sFactor = 1.000, reduct = 5e5;

            switch (fromCoordSys)               // Входна координатна система
            {
                case (ushort)CoordSys.CS1930:                        // От КС1930 към КС1950 6° ивица!
                    X_1950 = x_input;
                    Y_1950 = y_input;
                    H_1950 = z_input;
                    if (!from1930_XYH1950(fromCoordZone, fromCoordType, ref X_1950, ref Y_1950, ref H_1950))
                        return false;
                    // КС1942/83 6°		
                    X_4283 = X_1950;                // КС1942/83 - пространствени Декартови координати
                    Y_4283 = Y_1950;
                    Z_4283 = H_1950;
                    if (!XY1950_XY4283(5, ref X_4283, ref Y_4283))
                        return false;
                    if (!Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, 6, ref sFactor, ref reduct, ref X_4283, ref Y_4283))    // Гаус-Крюгерова проекция към Геодезически Географски координати
                        return false;
                    if (!BLH_XYZ((ushort)ellipsoid.KRASOVSKI, ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // BulRef89 XYZ		
                    X_WGS84 = X_4283;               // БулРеф89 - пространствени Декартови координати
                    Y_WGS84 = Y_4283;
                    Z_WGS84 = Z_4283;
                    if (!XYZ4283_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_4283;                // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_4283;
                    Z_2005 = Z_4283;
                    if (!XYZ4283_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.CS1950:                        // От КС1950 към КС1950 6° ивица!
                                                                     // КС1950 6°
                    X_1950 = x_input;
                    Y_1950 = y_input;
                    H_1950 = z_input;
                    if (!from1950_XYH1950(fromCoordZone, fromCoordType, ref X_1950, ref Y_1950, ref H_1950))
                        return false;
                    // КС1942/83 6°
                    X_4283 = X_1950;                // КС1942/83 - пространствени Декартови координати
                    Y_4283 = Y_1950;
                    Z_4283 = H_1950;
                    if (!XY1950_XY4283(5, ref X_4283, ref Y_4283))
                        return false;
                    if (!Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, 6, ref sFactor, ref reduct, ref X_4283, ref Y_4283))    // Гаус-Крюгерова проекция към Геодезически Географски координати
                        return false;
                    if (!BLH_XYZ((ushort)ellipsoid.KRASOVSKI, ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // BulRef89 XYZ
                    X_WGS84 = X_4283;               // БулРеф89 - пространствени Декартови координати
                    Y_WGS84 = Y_4283;
                    Z_WGS84 = Z_4283;
                    if (!XYZ4283_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_4283;                // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_4283;
                    Z_2005 = Z_4283;
                    if (!XYZ4283_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.CSSofiiska:                        // От Софийска към КС1950 6° ивица!
                    X_1950 = x_input;
                    Y_1950 = y_input;
                    H_1950 = z_input;
                    if (!fromSof_XYH1950(fromCoordZone, fromCoordType, ref X_1950, ref Y_1950))
                        return false;
                    // КС1942/83 6°
                    X_4283 = X_1950;                // КС1942/83 - пространствени Декартови координати
                    Y_4283 = Y_1950;
                    Z_4283 = H_1950;
                    if (!XY1950_XY4283(5, ref X_4283, ref Y_4283))
                        return false;
                    if (!Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, 6, ref sFactor, ref reduct, ref X_4283, ref Y_4283))    // Гаус-Крюгерова проекция към Геодезически Географски координати
                        return false;
                    if (!BLH_XYZ((ushort)ellipsoid.KRASOVSKI, ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // BulRef89 XYZ	
                    X_WGS84 = X_4283;               // БулРеф89 - пространствени Декартови координати
                    Y_WGS84 = Y_4283;
                    Z_WGS84 = Z_4283;
                    if (!XYZ4283_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_4283;                // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_4283;
                    Z_2005 = Z_4283;
                    if (!XYZ4283_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.CS1970:                        // От КС1970 към КС1950 6° ивица!
                    X_1950 = x_input;
                    Y_1950 = y_input;
                    H_1950 = z_input;
                    if (!from1970_XYH1950(fromCoordZone, ref X_1950, ref Y_1950))
                        return false;
                    // КС1942/83 6° XYZ
                    X_4283 = X_1950;                // КС1942/83
                    Y_4283 = Y_1950;
                    Z_4283 = H_1950;
                    if (!XY1950_XY4283(5, ref X_4283, ref Y_4283))
                        return false;
                    if (!Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, 6, ref sFactor, ref reduct, ref X_4283, ref Y_4283))    // Гаус-Крюгерова проекция към Геодезически Географски координати
                        return false;
                    if (!BLH_XYZ((ushort)ellipsoid.KRASOVSKI, ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // BulRef89 XYZ	
                    X_WGS84 = X_4283;               // БулРеф89 - пространствени Декартови координати
                    Y_WGS84 = Y_4283;
                    Z_WGS84 = Z_4283;
                    if (!XYZ4283_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_4283;                // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_4283;
                    Z_2005 = Z_4283;
                    if (!XYZ4283_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.CS1942:                        // От КС1942 към КС1942/83 6° ивица!
                    X_4283 = x_input;
                    Y_4283 = y_input;
                    Z_4283 = z_input;
                    if (!from1942_XYZ4283(fromCoordZone, fromCoordType, ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // КС1950 6° XYH
                    X_1950 = X_4283;                // КС1950
                    Y_1950 = Y_4283;
                    H_1950 = Z_4283;
                    if (!XYZ_BLH((ushort)ellipsoid.KRASOVSKI, ref X_1950, ref Y_1950, ref H_1950))
                        return false;
                    if (!Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1950, ref Y_1950))
                        return false;
                    if (!XY1950_XY4283(6, ref X_1950, ref Y_1950))
                        return false;
                    // BulRef89 XYZ	
                    X_WGS84 = X_4283;               // БулРеф89 - пространствени Декартови координати
                    Y_WGS84 = Y_4283;
                    Z_WGS84 = Z_4283;
                    if (!XYZ4283_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_4283;                // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_4283;
                    Z_2005 = Z_4283;
                    if (!XYZ4283_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.CS4283:                        // От КС1942/83 към КС1942/83 6° ивица!
                    X_4283 = x_input;
                    Y_4283 = y_input;
                    Z_4283 = z_input;
                    if (!from4283_XYZ4283(fromCoordZone, fromCoordType, ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // КС1950 6° XYH
                    X_1950 = X_4283;                // КС1950 към КС1950 6° ивица!
                    Y_1950 = Y_4283;
                    H_1950 = Z_4283;
                    if (!XYZ_BLH((ushort)ellipsoid.KRASOVSKI, ref X_1950, ref Y_1950, ref H_1950))
                        return false;
                    if (!Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1950, ref Y_1950))
                        return false;
                    if (!XY1950_XY4283(6, ref X_1950, ref Y_1950))
                        return false;
                    // BulRef89 XYZ	
                    X_WGS84 = X_4283;               // БулРеф89 - пространствени Декартови координати
                    Y_WGS84 = Y_4283;
                    Z_WGS84 = Z_4283;
                    if (!XYZ4283_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_4283;                // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_4283;
                    Z_2005 = Z_4283;
                    if (!XYZ4283_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.ETRS89:                        // От БулРеф89 към Геоцентрични Декартови координати
                    X_WGS84 = x_input;
                    Y_WGS84 = y_input;
                    Z_WGS84 = z_input;
                    if (!fromBulRef89_XYZWGS84(fromCoordZone, fromCoordType, ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС1942/83 6° XYZ
                    X_4283 = X_WGS84;               // КС1942/83 - пространствени Декартови координати
                    Y_4283 = Y_WGS84;
                    Z_4283 = Z_WGS84;
                    if (!XYZ89_XYZ4283(ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // КС1950 6° XYH
                    X_1950 = X_4283;                // КС1950 към КС1950 6° ивица!
                    Y_1950 = Y_4283;
                    H_1950 = Z_4283;
                    if (!XYZ_BLH((ushort)ellipsoid.KRASOVSKI, ref X_1950, ref Y_1950, ref H_1950))
                        return false;
                    if (!Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1950, ref Y_1950))
                        return false;
                    if (!XY1950_XY4283(6, ref X_1950, ref Y_1950))
                        return false;
                    // КС2005 XYZ
                    X_2005 = X_WGS84;               // КС2005 - пространствени Декартови координати
                    Y_2005 = Y_WGS84;
                    Z_2005 = Z_WGS84;
                    if (!XYZ89_XYZ2005(ref X_2005, ref Y_2005, ref Z_2005))
                        return false;

                    break;
                case (ushort)CoordSys.CS2000:                        // От Ламбертова проекция върху елипсоид WGS84 - КС2000
                    return false;

                    break;
                case (ushort)CoordSys.CS2005:                        // От КС2005 UTM и Ламбертова към Геоцентрични Декартови координати

                    X_2005 = x_input;               // КС2005 - пространствени Декартови координати
                    Y_2005 = y_input;
                    Z_2005 = z_input;
                    if (!from2005_XYZ2005(fromCoordSys, fromCoordZone, fromCoordType, ref X_2005, ref Y_2005, ref Z_2005))
                        return false;
                    X_WGS84 = X_2005;               // БулРеф89
                    Y_WGS84 = Y_2005;
                    Z_WGS84 = Z_2005;
                    if (!XYZ2005_XYZ89(ref X_WGS84, ref Y_WGS84, ref Z_WGS84))
                        return false;
                    // КС1942/83 6° XYZ
                    X_4283 = X_2005;                // КС1942/83 - пространствени Декартови координати
                    Y_4283 = Y_2005;
                    Z_4283 = Z_2005;
                    if (!XYZ2005_XYZ4283(ref X_4283, ref Y_4283, ref Z_4283))
                        return false;
                    // КС1950 6° XYH
                    X_1950 = X_4283;                // КС1950 към КС1950 6° ивица!
                    Y_1950 = Y_4283;
                    H_1950 = Z_4283;
                    if (!XYZ_BLH((ushort)ellipsoid.KRASOVSKI, ref X_1950, ref Y_1950, ref H_1950))
                        return false;
                    if (!Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1950, ref Y_1950))
                        return false;
                    if (!XY1950_XY4283(6, ref X_1950, ref Y_1950))
                        return false;

                    break;
                default:
                    return false;
                    break;
            };

            switch (toCoordSys)                 // Изходна координатна система
            {

                case (ushort)CoordSys.CS2005:                        // към КС2005 UTM проекция и Ламбертова проекция
                    x_output = X_2005;
                    y_output = Y_2005;
                    z_output = Z_2005;
                    to2005(toCoordSys, toCoordZone, toCoordType, toCurrentZone, ref x_output, ref y_output, ref z_output);
                    break;
                default:
                    return false;
                    break;
            }

            // Височинни системи
            double B84 = X_WGS84, L84 = Y_WGS84, dh = Z_WGS84;
            XYZ_BLH((ushort)ellipsoid.WGS84, ref B84, ref L84, ref dh);           // Трансформация на правоъгълни пространствени координати (X, Y, Z) в геодезически географски координати (B, L, h)

            //	z_output += dh;
            if (toCoordType > 0)                    // Ако не е XYZ!
                z_output = dh;

            return true;
        }

        // Трансформация между КС1930 и КС1950 (стара)
        bool BL1930_BL1950(double B30, double L30, ref double B50, ref double L50)   // Трансформацията от Географски координати - система „1930 г.” в Географски координати в система „1950 г.”
        {
            double a, alfa, e2, ee2;
            a = Semi_major_axis[(ushort)ellipsoid.HAYFORD];           // Екваториален радиус на Земята (голяма полуос) за Красовски
            alfa = 1 / Inverse_flattening[(ushort)ellipsoid.HAYFORD]; // Полярна сплеснатост на Земята за Красовски
            e2 = 2 * alfa - alfa * alfa;                // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                        // Втори ексцентрицитет на втора степен - e'
            double B0h, L0h, dB, dL;
            B0h = 42.0 + 33.0 / 60 + 54.5526 / 3600;        // Черни връх по Хейфорд 1930
            L0h = 23.0 + 16.0 / 60 + 51.9603 / 3600;

            // Диференциални разлики межди двата елипсоида за Черни връх
            double da = (Semi_major_axis[(ushort)ellipsoid.KRASOVSKI] - Semi_major_axis[(ushort)ellipsoid.HAYFORD]) / Semi_major_axis[(ushort)ellipsoid.HAYFORD]; // разлика в оста []
            double dAlf = 1 / Inverse_flattening[(ushort)ellipsoid.KRASOVSKI] - 1 / Inverse_flattening[(ushort)ellipsoid.HAYFORD];              // разлика в сплеснатоста []
            double dBi = (42.0 + 33.0 / 60 + 51.1286 / 3600 - (42.0 + 33.0 / 60 + 54.5526 / 3600));               // разлика в ширина [°]
            double dLi = (23.0 + 16.0 / 60 + 48.3083 / 3600 - (23.0 + 16.0 / 60 + 51.9603 / 3600));               // разлика в дължина [°]
            double dA = (309.0 + 55.0 / 60 + 21.7520 / 3600 - (309.0 + 55.0 / 60 + 21.752 / 3600));               // разлика в азимут [°]
            double dS = 0.0;                                                                        // разлика в разстояния [m]

            dB = (B30 - B0h) * _PI / 180;                               // В аналитична мярка - Радиани
            dL = (L30 - L0h) * _PI / 180;

            double B0 = B0h * _PI / 180;                                // В аналитична мярка - Радиани

            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B0), 2));           // Напречен радиус на кривина на референтния елипсоид
            double t = Math.Tan(B0);                                    //
            double eta2 = ee2 * Math.Pow(Math.Cos(B0), 2);                  //
                                                                            // Метод на разгъването!
            double a1 = 1 - 3 * t * eta2 * dB - Math.Cos(B0) * Math.Cos(B0) * (1 + t * t) * dL * dL / 2;                            // *dBi
            double a3 = dB - t * Math.Cos(B0) * Math.Cos(B0) * dL * dL / 2 - Math.Cos(B0) * Math.Cos(B0) * dB * dL * dL / 3;                // -> 0 *dS = 0
            double a4 = -(1 + eta2) * Math.Cos(B0) * dL + Math.Cos(B0) * Math.Cos(B0) * Math.Cos(B0) * (1 + t * t) * dL * dL * dL / 6;          // -> 0 *dA = 0
            double a5 = -a3;                                                                            // *da
            double a6 = (4 - 2 * t * t + 2 * eta2 + 7 * t * t * eta2 - t * t * t * t * eta2) * Math.Cos(B0) * Math.Cos(B0) * dB / 2 -       // *dAlf
                3 * t * Math.Cos(B0) * Math.Cos(B0) * dB * dB + t * t * t * Math.Pow(Math.Cos(B0), 4) * dL * dL / 2 +
                (-1 + t * t) * Math.Cos(B0) * Math.Cos(B0) * dB * dB * dB + (-1 + t * t) * Math.Pow(Math.Cos(B0), 4) * dB * dL * dL / 3;

            double b1 = t * (1 - eta2) * dL + (1 + t * t) * dB * dL + t * (1 + t * t) * dB * dB * dL -                      // *dBi
                t * (1 + t * t) * Math.Cos(B0) * Math.Cos(B0) * dL * dL * dL / 6;
            double b3 = dL + t * dB * dL + (2 + 3 * t * t) * dB * dB * dL / 3 - t * t * Math.Cos(B0) * Math.Cos(B0) * dL * dL * dL / 6;     // *dS = 0
            double b4 = (1 - eta2) * dB / Math.Cos(B0) + t * dB * dB / Math.Cos(B0) - t * Math.Cos(B0) * dL * dL / 2 +              // *dA = 0
                (1 + 3 * t * t) * dB * dB * dB / Math.Cos(B0) / 3 - Math.Cos(B0) * dB * dL * dL / 2;
            double b5 = -b3;                                                                            // *da
            double b6 = -(2 * t * t - t * t * eta2 + t * t * t * t * eta2) * dL * Math.Cos(B0) * Math.Cos(B0) / 2 -                     // *dAlf
                t * t * t * Math.Cos(B0) * Math.Cos(B0) * dB * dL + (2 - 2 * t * t - 3 * t * t * t * t) * Math.Cos(B0) * Math.Cos(B0) * dB * dB * dL / 3 +
                t * t * t * t * Math.Cos(B0) * Math.Cos(B0) * Math.Cos(B0) * Math.Cos(B0) * dL * dL * dL / 6;

            double dBk, dLk;
            dBk = a1 * dBi + a3 * dS + a4 * dA + a5 * da * 180 / _PI + a6 * dAlf * 180 / _PI;
            B50 = B30 + dBk;
            dLk = dLi + b1 * dBi + b3 * dS + b4 * dA + b5 * da * 180 / _PI + b6 * dAlf * 180 / _PI;
            L50 = L30 + dLk;

            return true;
        }
        bool BL1950_BL1930(double B50, double L50, ref double B30, ref double L30)   // Трансформацията от Географски координати - система „1950 г.” в Географски координати в система „1930 г.”
        {
            double a, alfa, e2, ee2;
            a = Semi_major_axis[(ushort)ellipsoid.KRASOVSKI];         // Екваториален радиус на Земята (голяма полуос) за Красовски
            alfa = 1 / Inverse_flattening[(ushort)ellipsoid.KRASOVSKI];   // Полярна сплеснатост на Земята за Красовски
                                                                          //	a = 6378245.;						// Красовски 1942	// ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1942 г. (ГРС  42)/83
                                                                          //	alfa = 1/298.3;						// Красовски 1942	// ГЕОДЕЗИЧЕСКА РЕФЕРЕНТНА СИСТЕМА 1942 г. (ГРС  42)/83
            e2 = 2 * alfa - alfa * alfa;                // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                    // Втори ексцентрицитет на втора степен - e'
            double B0k, L0k, dB, dL;
            // А = 309° 55' 21.752"
            B0k = 42.0 + 33.0 / 60 + 51.1286 / 3600;   // Черни връх по Красовски 1950
            L0k = 23.0 + 16.0 / 60 + 48.3083 / 3600;
            // А = 309° 55' 21.7520"
            // Диференциални разлики межди двата елипсоида за Черни връх : Хейфорд - Красовски
            double da = (Semi_major_axis[(ushort)ellipsoid.HAYFORD] - Semi_major_axis[(ushort)ellipsoid.KRASOVSKI]) / Semi_major_axis[(ushort)ellipsoid.KRASOVSKI];   // разлика в оста []
            double dAlf = 1 / Inverse_flattening[(ushort)ellipsoid.HAYFORD] - 1 / Inverse_flattening[(ushort)ellipsoid.KRASOVSKI];              // разлика в сплеснатоста []
            double dBi = (42.0 + 33.0 / 60 + 54.5526 / 3600 - (42.0 + 33.0 / 60 + 51.1286 / 3600));               // разлика в ширина [°]
            double dLi = (23.0 + 16.0 / 60 + 51.9603 / 3600 - (23.0 + 16.0 / 60 + 48.3083 / 3600));               // разлика в дължина [°]
            double dA = (309.0 + 55.0 / 60 + 21.752 / 3600 - (309.0 + 55.0 / 60 + 21.7520 / 3600));               // разлика в азимут [°]
            double dS = 0.0;                                                                        // разлика в разстояния [m]

            dB = (B50 - B0k) * _PI / 180;                               //  В аналитична мярка - Радиани
            dL = (L50 - L0k) * _PI / 180;

            double B0 = B0k * _PI / 180;                                // В аналитична мярка - Радиани

            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B0), 2));           // Напречен радиус на кривина на референтния елипсоид
            double t = Math.Tan(B0);                                    //
            double eta2 = ee2 * Math.Pow(Math.Cos(B0), 2);                  //
                                                                            // Метод на разгъването!
            double a1 = 1 - 3 * t * eta2 * dB - Math.Cos(B0) * Math.Cos(B0) * (1 + t * t) * dL * dL / 2;                            // *dBi
            double a3 = dB - t * Math.Cos(B0) * Math.Cos(B0) * dL * dL / 2 - Math.Cos(B0) * Math.Cos(B0) * dB * dL * dL / 3;                // -> 0 *dS = 0
            double a4 = -(1 + eta2) * Math.Cos(B0) * dL + Math.Cos(B0) * Math.Cos(B0) * Math.Cos(B0) * (1 + t * t) * dL * dL * dL / 6;          // -> 0 *dA = 0
            double a5 = -a3;                                                                            // *da
            double a6 = (4 - 2 * t * t + 2 * eta2 + 7 * t * t * eta2 - t * t * t * t * eta2) * Math.Cos(B0) * Math.Cos(B0) * dB / 2 -       // *dAlf
                3 * t * Math.Cos(B0) * Math.Cos(B0) * dB * dB + t * t * t * Math.Pow(Math.Cos(B0), 4) * dL * dL / 2 +
                (-1 + t * t) * Math.Cos(B0) * Math.Cos(B0) * dB * dB * dB + (-1 + t * t) * Math.Pow(Math.Cos(B0), 4) * dB * dL * dL / 3;

            double b1 = t * (1 - eta2) * dL + (1 + t * t) * dB * dL + t * (1 + t * t) * dB * dB * dL -                      // *dBi
                t * (1 + t * t) * Math.Cos(B0) * Math.Cos(B0) * dL * dL * dL / 6;
            double b3 = dL + t * dB * dL + (2 + 3 * t * t) * dB * dB * dL / 3 - t * t * Math.Cos(B0) * Math.Cos(B0) * dL * dL * dL / 6;     // *dS = 0
            double b4 = (1 - eta2) * dB / Math.Cos(B0) + t * dB * dB / Math.Cos(B0) - t * Math.Cos(B0) * dL * dL / 2 +              // *dA = 0
                (1 + 3 * t * t) * dB * dB * dB / Math.Cos(B0) / 3 - Math.Cos(B0) * dB * dL * dL / 2;
            double b5 = -b3;                                                                            // *da
            double b6 = -(2 * t * t - t * t * eta2 + t * t * t * t * eta2) * dL * Math.Cos(B0) * Math.Cos(B0) / 2 -                     // *dAlf
                t * t * t * Math.Cos(B0) * Math.Cos(B0) * dB * dL + (2 - 2 * t * t - 3 * t * t * t * t) * Math.Cos(B0) * Math.Cos(B0) * dB * dB * dL / 3 +
                t * t * t * t * Math.Cos(B0) * Math.Cos(B0) * Math.Cos(B0) * Math.Cos(B0) * dL * dL * dL / 6;

            double dBk, dLk;
            dBk = a1 * dBi + a3 * dS + a4 * dA + a5 * da * 180 / _PI + a6 * dAlf * 180 / _PI;
            B30 = B50 + dBk;
            dLk = dLi + b1 * dBi + b3 * dS + b4 * dA + b5 * da * 180 / _PI + b6 * dAlf * 180 / _PI;
            L30 = L50 + dLk;

            return true;
        }
        bool XY1930_XY1950(short fromto, ref double X, ref double Y)             // Трансформацията от проекционните координати - система „1930 г.” в триградусовите Гаусови координати в система „1950 г.” и обратно
        {
            ushort zone;
            if (Y >= 1e6)               // номер на ивицата 8 или 9, две 3° зони – с осеви меридиани 24° и 27°, мащаб по меридиана 0.9999
                zone = (ushort)(Y / 1e6);
            else
                return false;           // Номера на ивицата се определя от редукционното число! Връща входните координати

            double x0, y0, dx, dy;

            switch (zone)
            {
                case 8:                     // == 8 -> 8е6
                    dx = (X - 4700000.0) * 0.00001;
                    dy = (Y - 8500000.0) * 0.00001;
                    x0 = +363.346
                        + dx * 10.001
                        - dy * 1.1796
                        - dy * dx * 0.0206
                        + (dy * 0.00035 + dx * 0.00014) * dy * dx
                        - dy * dy * dy * 0.00005;
                    X += fromto * x0;               //???!!!  +- от 50 към 30 тогава -

                    y0 = ((dx * 0.0014 + dy * 0.0017) * dy - dx * dx * 0.0017) * dy
                        + (dx * 0.0103 + 1.1796) * dx
                        - dy * dy * 0.0103
                        + dy * 10.001
                        - 82.645;
                    Y += fromto * y0;               //???!!!  +- от 50 към 30 тогава -

                    return true;
                case 9:                         // ==9 -> 9e6
                    dx = (X - 4700000.0) * 0.00001;
                    dy = (Y - 9500000.0) * 0.00001;
                    x0 = +363.372
                        + dx * 9.9994
                        - dy * 1.116
                        - dy * dx * 0.0206
                        + (dy * 0.00035 + dx * 0.00014) * dy * dx
                        - dy * dy * dy * 0.00005;
                    X += fromto * x0;               //???!!!  +- от 50 към 30 тогава -

                    y0 = ((dx * 0.0014 + dy * 0.0017) * dy - dx * dx * 0.0017) * dy
                        + (dx * 0.0108 + 1.1158) * dx
                        - dy * dy * 0.0098
                        + dy * 10.001
                        - 79.2;
                    Y += fromto * y0;               //???!!!  +- от 50 към 30 тогава -

                    return true;
                default:
                    return false;               // Връща входните координати
            }
            return true;
        }
        bool XY1930_XY1950_(short fromto, ref double X, ref double Y)            // Трансформацията от проекционните координати - система „1930 г.” в триградусовите Гаусови координати в система „1950 г.” и обратно
        {
            ushort zone;
            if (Y >= 1e6)               // номер на ивицата 8 или 9, две 3° зони – с осеви меридиани 24° и 27°, мащаб по меридиана 0.9999
                zone = (ushort)(Y / 1e6);
            else
                return false;           // Номера на ивицата се определя от редукционното число!

            double a00, a10, a01, a20, a11, a02, a30, a21, a12, a03,
                    b00, b10, b01, b20, b11, b02, b30, b21, b12, b03,
                    x0, y0;
            switch (zone)
            {
                case 8:                         // Осев меридиан 24°
                    a00 = 363.346; a10 = 10.0010; a01 = -1.1796; a20 = -0.000; a11 = -0.0206;
                    a02 = -0.000; a30 = -0.000; a21 = 0.00014; a12 = 0.00035; a03 = 0.00005;
                    b00 = -82.645; b10 = 1.1796; b01 = 10.0010; b20 = 0.0103; b11 = -0.000;
                    b02 = -0.0103; b30 = -0.000; b21 = -0.0017; b12 = 0.0014; b03 = 0.0017;
                    x0 = 4700000; y0 = 8500000;
                    break;
                case 9:                         // Осев меридиан 27°
                    a00 = 363.372; a10 = 9.9994; a01 = -1.1160; a20 = -0.000; a11 = -0.0206;
                    a02 = -0.000; a30 = -0.000; a21 = 0.00014; a12 = 0.00035; a03 = -0.00005;
                    b00 = -79.200; b10 = 1.1158; b01 = 10.0010; b20 = 0.0108; b11 = -0.000;
                    b02 = -0.0098; b30 = -0.000; b21 = -0.0017; b12 = 0.0014; b03 = 0.0017;
                    x0 = 4700000; y0 = 9500000;
                    break;
                default:
                    return false;               // Връща входните координати
            }
            // Трансформация между две конформни проекции със степенни редове се въвеждат условия, а именно да бъдат изпълнени Коши-Римановите частни диференциални уравнения
            // a10 = b01;					// a01 =-b10;
            // a20 =-a02 = b11/2;			// b20 =-b02 = a11/2;
            // a30 =-a03 = b12/2 =-b21/2;	// b30 =-b03 =-a12/2 = a21/2;
            double dx, dy;
            dx = (X - x0) * 1e-5;
            dy = (Y - y0) * 1e-5;
            X += fromto * (a00 + a10 * dx + a01 * dy + a20 * dx * dx + a11 * dx * dy + a02 * dy * dy +
                        a30 * dx * dx * dx + a21 * dx * dx * dy + a12 * dx * dy * dy + a03 * dy * dy * dy);

            Y += fromto * (b00 + b10 * dx + b01 * dy + b20 * dx * dx + b11 * dx * dy + b02 * dy * dy +
                        b30 * dx * dx * dx + b21 * dx * dx * dy + b12 * dx * dy * dy + b03 * dy * dy * dy);

            return true;
        }
        bool XY1930_XY1950_Affine(short fromto, ref double X, ref double Y)      // Трансформацията от проекционните координати - система „1930 г.” в триградусовите Гаусови координати в система „1950 г.” и обратно
        {
            double x0;
            double y0;
            ushort zone;
            if (Y >= 1e6)               // номер на ивицата 8 или 9, две 3° зони – с осеви меридиани 24° и 27°, мащаб по меридиана 0.9999
                zone = (ushort)(Y / 1e6);
            else
                return false;

            if (zone == 8)                     // Основен меридиан 24 зона 8
            {
                x0 = X - 4727894.74;
                y0 = Y - 8502631.58;
                X = x0 * 1.0001 + 4728260.84 - y0 * 0.00001183;
                Y = y0 * 1.0001 + x0 * 0.00001181 + 8502549.52;
            }
            else                                          // Основен меридиан 27 зона 9
            {
                x0 = X - 4713030.30;
                y0 = Y - 9460606.06;
                X = x0 * 1.0001 + 4713395.42 - y0 * 0.0000111;
                Y = y0 * 1.0001 + x0 * 0.0000112 + 9460523.06;
            }
            return true;
        }
        bool XY1930_XY1950_Polynom(short fromto, ref double X, ref double Y)     // Трансформацията от проекционните координати - система „1930 г.” в триградусовите Гаусови координати в система „1950 г.” и обратно
        {
            double x0;
            double y0;
            ushort zone;
            if (Y >= 1e6)               // номер на ивицата 8 или 9, две 3° зони – с осеви меридиани 24° и 27°, мащаб по меридиана 0.9999
                zone = (ushort)(Y / 1e6);
            else
                return false;

            if (zone == 8)                     // основен маридиан 24 зона 8
            {
                x0 = X - 4727894.737;
                y0 = Y - 8502631.579;
                X = x0 * 1.0001
                    + 4728260.842
                    - y0 * 0.000011857
                    - y0 * x0 * 2.416596e-12
                    - x0 * x0 * 3.51652e-13
                    + y0 * y0 * 2.307465e-14;

                Y = (x0 * 2.449005e-12 + y0 * 8.379404e-13 + 0.00001183) * x0
                   + y0 * 1.0001
                   + 8502549.518999999
                   - y0 * y0 * 1.117856e-12;
            }
            else                                          // Основен меридиан 27 зона 9
            {
                x0 = (X - 4713030.303) * 0.001;
                y0 = (Y - 9460606.061) * 0.001;
                X = x0 * 1.0001
                   + 4713395.417
                   - y0 * 0.0112254
                   - y0 * x0 * 0.000003681
                   + x0 * x0 * 0.00000261
                   + y0 * y0 * 0.000000063;

                Y = x0 * 0.0115884
                   + y0 * 1.0001
                   + 9460523.056
                   - y0 * x0 * 0.0000000427
                   + x0 * 0.0000009629999999999999
                   - y0 * y0 * 0.000000197;
            }

            return true;
        }

        // Трансформация между КС1970 и КС1950 (стара?)
        bool BL1950_XY1970(ushort zone, ref double X70, ref double Y70)  // Трансформация на географски координати от система “1950 г.” в проекционни координати в система “1970 г.”
        {
            double a, alfa, e2, ee2;                // Константи на елипсоида
                                                    //	a=Semi_major_axis[KRASOVSKI];			// Екваториален радиус на Земята (голяма полуос) за Красовски
                                                    //	alfa=1/Inverse_flattening[KRASOVSKI];	// Полярна сплеснатост на Земята за Красовски
            a = 6378245.0;                           // Красовски 1942
            alfa = 1 / 298.3;                           // Красовски 1942
            e2 = 2 * alfa - alfa * alfa;                    // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                        // Втори ексцентрицитет на втора степен - e'
            double dF, dL, F0, L0, dA, X0, Y0, B, L;
            B = X70;
            L = Y70;
            //	X70 = Y70 = 0;
            //	Зони	F0			L0			dA, 		X0 m,		Y0 m, Според Лесидренски има и dF & dL транслация на координатите
            //	K-3	43°27’25”	23°14’15”	-0.027651055°	4724463.651	8500000.000
            //	K-5	42°28’45”	26°25’35”	-0.0246105°		4638981.029	9500000.000
            //	K-7	43°33’48”	26°11’13”	 0.030881916°	4723911.711	9500000.000
            //	K-9	42°17’35”	23°20’33”	 0.052087361°	4558613.089	8500000.000
            switch (zone)
            {
                case 3:                             // K-3
                    F0 = 43.0 + 27.0 / 60 + 25.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 23.0 + 14.0 / 60 + 15.0 / 3600;
                    dA = -0.027651055;              // ъглите на завъртане около тях
                    X0 = 4724463.651;               // стойност на абсцисата по стандартния паралел
                    Y0 = 8500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                case 5:                             // K-5
                    F0 = 42.0 + 28.0 / 60 + 45.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 26.0 + 25.0 / 60 + 35.0 / 3600;
                    dA = -0.0246105;                // ъглите на завъртане около тях
                    X0 = 4638981.029;               // стойност на абсцисата по стандартния паралел
                    Y0 = 9500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                case 7:                             // K-7
                    F0 = 43.0 + 33.0 / 60 + 48.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 26.0 + 11.0 / 60 + 13.0 / 3600;
                    dA = 0.030881916;               // ъглите на завъртане около тях
                    X0 = 4723911.711;               // стойност на абсцисата по стандартния паралел
                    Y0 = 9500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                case 9:                             // K-9
                    F0 = 42.0 + 17.0 / 60 + 35.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 23.0 + 20.0 / 60 + 33.0 / 3600;
                    dA = 0.052087361;               // ъглите на завъртане около тях
                    X0 = 4558613.089;               // стойност на абсцисата по стандартния паралел
                    Y0 = 8500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                default:
                    return false;                   // Ще върне входните координати BL в 1950
            }
            B = B * _PI / 180;                      // геодезически географски координати в система "1950 г."
            L = L * _PI / 180;                      // в аналитична мярка (РАДИАНИ)
            dA = dA * _PI / 180;
            F0 = F0 * _PI / 180;
            L0 = L0 * _PI / 180;
            dF = B - F0;
            dL = L - L0;

            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(F0), 2));       // Напречен радиус на кривина на референтния елипсоид
            double t = Math.Tan(F0);                                //
            double eta2 = ee2 * Math.Pow(Math.Cos(F0), 2);              //
                                                                        // Метод на разгъване при трансформация между два референтни елипсоида точност 0,001"
            double g01 = -dA * (1 + eta2) * Math.Cos(F0);
            double g11 = 3 * dA * t * eta2 * Math.Cos(F0);
            double g03 = dA * (1 + t * t) * Math.Pow(Math.Cos(F0), 3) / 6;

            double h10 = dA * (1 - eta2 + eta2 * eta2) / Math.Cos(F0);  // Тука разлика с GPS + eta2*eta2
            double h20 = dA * t * (1 - eta2 / 2) / Math.Cos(F0);
            double h02 = -dA * t * Math.Cos(F0) / 2;
            double h30 = dA * (1 + 3 * t * t) / 3 / Math.Cos(F0);
            double h12 = -dA * (1 + t * t) * Math.Cos(F0) / 2;
            // Загуба на точност 0.001" много! завъртане около фиктивната точка
            double F1 = B + g01 * dL + g11 * dF * dL + g03 * dL * dL * dL;
            double L1 = L + h10 * dF + h20 * dF * dF + h02 * dL * dL + h30 * dF * dF * dF + h12 * dF * dL * dL;
            dF = F1 - F0;
            dL = L1 - L0;
            // Конична конформна проекция - точност 0.00002"
            double a10 = N * (1 - eta2 + eta2 * eta2 - eta2 * eta2 * eta2);
            double a20 = 3 * N * t * (eta2 - 2 * eta2 * eta2) / 2.0;
            double a02 = N * t * Math.Pow(Math.Cos(F0), 2) / 2.0;
            double a30 = N * (1 + eta2 - 3 * t * t * eta2 - 3 * eta2 * eta2 + 21 * t * t * eta2 * eta2) / 6.0;
            double a12 = N * (-t * t + t * t * eta2 - t * t * eta2 * eta2) * Math.Pow(Math.Cos(F0), 2) / 2.0;
            double a40 = N * t * (1 - eta2) / 24.0;
            double a22 = -3 * N * t * t * t * eta2 * Math.Pow(Math.Cos(F0), 2) / 4.0;
            double a04 = -N * t * t * t * Math.Pow(Math.Cos(F0), 4) / 24.0;
            double a50 = N * (5 + 3 * t * t) / 120.0;
            double a32 = -N * t * t * Math.Pow(Math.Cos(F0), 2) / 12.0;
            double a14 = N * t * t * t * t * Math.Pow(Math.Cos(F0), 4) / 24.0;

            double b01 = N * Math.Cos(F0);
            double b11 = N * t * (-1 + eta2 - eta2 * eta2) * Math.Cos(F0);
            double b21 = 3 * N * (-t * t * eta2 + 2 * t * t * eta2 * eta2) * Math.Cos(F0) / 2.0;         // 3/2 -> 1/2 Математическа Геодезия 7.22 стр.309
            double b03 = -N * t * t * Math.Pow(Math.Cos(F0), 3) / 6.0;
            double b31 = N * t * (-1 - eta2 + 3 * t * t * eta2) * Math.Cos(F0) / 6.0;
            double b13 = N * t * (t * t - t * t * eta2) * Math.Pow(Math.Cos(F0), 3) / 6.0;
            double b41 = -N * t * t * Math.Cos(F0) / 24.0;
            double b05 = N * t * t * t * t * Math.Pow(Math.Cos(F0), 5) / 120.0;
            // Проекциони координати X - север, Y - изток в метри в КС1970
            X70 = X0 + a10 * dF + a20 * dF * dF + a02 * dL * dL + a30 * dF * dF * dF + a12 * dF * dL * dL +
                a40 * dF * dF * dF * dF + a22 * Math.Pow((dF * dL), 2) + a04 * Math.Pow(dL, 4) + a50 * Math.Pow(dF, 5) +
                a32 * Math.Pow(dF, 3) * Math.Pow(dL, 2) + a14 * dF * Math.Pow(dL, 4);
            Y70 = Y0 + b01 * dL + b11 * dF * dL + b21 * dF * dF * dL + b03 * Math.Pow(dL, 3) +
                b31 * Math.Pow(dF, 3) * dL + b13 * dF * Math.Pow(dL, 3) + b41 * Math.Pow(dF, 4) * dL + b05 * Math.Pow(dL, 5);

            return true;
        }
        bool XY1970_BL1950(ushort zone, ref double B, ref double L)  // Трансформация на проекционни координати от система “1970 г.” в географски координати в система “1950 г.” 
        {
            double a, alfa, e2, ee2;                // Константи на елипсоида
                                                    //	a=Semi_major_axis[KRASOVSKI];			// Екваториален радиус на Земята (голяма полуос) за Красовски
                                                    //	alfa=1/Inverse_flattening[KRASOVSKI];	// Полярна сплеснатост на Земята за Красовски
            a = 6378245.0;                           // Красовски 1942
            alfa = 1 / 298.3;                           // Красовски 1942
            e2 = 2 * alfa - alfa * alfa;                    // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                        // Втори ексцентрицитет на втора степен - e'
            double dX, dY, F0, L0, dA, X0, Y0, dF, dL, X, Y;
            X = B;
            Y = L;
            //	B = L = 0;
            //	Зони	F0			L0			dA, 			X0 m ,	 Y0 m
            //	K-3	43°27’25”	23°14’15”	-0.027651055°	4724463.651	8500000.000
            //	K-5	42°28’45”	26°25’35”	-0.0246105°		4638981.029	9500000.000
            //	K-7	43°33’48”	26°11’13”	0.030881916°	4723911.711	9500000.000
            //	K-9	42°17’35”	23°20’33”	0.052087361°	4558613.089	8500000.000
            switch (zone)
            {
                case 3:                             // Зона на КС1970 K-3
                    F0 = 43.0 + 27.0 / 60 + 25.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 23.0 + 14.0 / 60 + 15.0 / 3600;
                    dA = -0.027651055;              // ъглите на завъртане около тях
                    X0 = 4724463.651;               // стойност на абсцисата по стандартния паралел
                    Y0 = 8500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                case 5:                             // Зона на КС1970 K-5
                    F0 = 42.0 + 28.0 / 60 + 45.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 26.0 + 25.0 / 60 + 35.0 / 3600;
                    dA = -0.0246105;                // ъглите на завъртане около тях
                    X0 = 4638981.029;               // стойност на абсцисата по стандартния паралел
                    Y0 = 9500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                case 7:                             // Зона на КС1970 K-7
                    F0 = 43.0 + 33.0 / 60 + 48.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 26.0 + 11.0 / 60 + 13.0 / 3600;
                    dA = 0.030881916;               // ъглите на завъртане около тях
                    X0 = 4723911.711;               // стойност на абсцисата по стандартния паралел
                    Y0 = 9500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                case 9:                             // Зона на КС1970 K-9
                    F0 = 42.0 + 17.0 / 60 + 35.0 / 3600; // географски координати на фиктивните централни точки
                    L0 = 23.0 + 20.0 / 60 + 33.0 / 3600;
                    dA = 0.052087361;               // ъглите на завъртане около тях
                    X0 = 4558613.089;               // стойност на абсцисата по стандартния паралел
                    Y0 = 8500000.000;               // стойност на ордината във фиктивната централна точка
                    break;
                default:
                    return false;                   // Ще върне входните координати XY в 1970!
            }
            dA = dA * _PI / 180;                    // В аналитична мярка (РАДИАНИ)
            F0 = F0 * _PI / 180;
            L0 = L0 * _PI / 180;

            dX = X - X0;
            dY = Y - Y0;

            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(F0), 2));           // Напречен радиус на кривина на референтния елипсоид
            double t = Math.Tan(F0);                                //
            double eta2 = ee2 * Math.Cos(F0) * Math.Cos(F0);                //
            dX /= N;
            dY /= N;
            // Конична конформна проекция - точност 0.00002"
            double c10 = (1 + eta2);
            double c20 = -3 * t * (eta2 + eta2 * eta2) / 2;
            double c02 = -t * (1 + eta2) / 2;
            double c30 = (-1 - 5 * eta2 + 3 * t * t * eta2 - 7 * eta2 * eta2 + 18 * t * t * eta2 * eta2) / 6;
            double c12 = t * t * (-1 + 2 * eta2 + 3 * eta2 * eta2) / 2;
            double c40 = t * (-1 + 26 * eta2) / 24;
            double c22 = t * (1 - 2 * t * t + 5 * eta2 + t * t * eta2) / 4;
            double c04 = t * t * t * (1 - 2 * eta2) / 8;
            double c50 = (5 - 3 * t * t) / 120;
            double c32 = (2 * t * t - 3 * t * t * t * t) / 6;
            double c14 = (-t * t + 3 * t * t * t * t) / 8;

            double d01 = 1 / Math.Cos(F0);
            double d11 = t / Math.Cos(F0);
            double d21 = t * t / Math.Cos(F0);
            double d03 = -t * t / 3 / Math.Cos(F0);
            double d31 = t * t * t / Math.Cos(F0);
            double d13 = -t * t * t / Math.Cos(F0);
            double d41 = t * t * t * t / Math.Cos(F0);
            double d23 = -2 * t * t * t * t / Math.Cos(F0);
            double d05 = t * t * t * t / 5 / Math.Cos(F0);

            dF = c10 * dX + c20 * dX * dX + c02 * dY * dY + c30 * Math.Pow(dX, 3) + c12 * dX * dY * dY +
                c40 * Math.Pow(dX, 4) + c22 * dX * dX * dY * dY + c04 * Math.Pow(dY, 4) + c50 * Math.Pow(dX, 5) +
                c32 * Math.Pow(dX, 3) * Math.Pow(dY, 2) + c14 * dX * Math.Pow(dY, 4);

            dL = d01 * dY + d11 * dX * dY + d21 * dX * dX * dY + d03 * Math.Pow(dY, 3) + d31 * Math.Pow(dX, 3) * dY +
                d13 * dX * Math.Pow(dY, 3) + d41 * Math.Pow(dX, 4) * dY + d23 * Math.Pow(dX, 2) * Math.Pow(dY, 3) + d05 * Math.Pow(dY, 5);
            double F1 = F0 + dF;
            double L1 = L0 + dL;
            // Метод на разгъване при трансформация между два референтни елипсоида точност 0,001"
            double g01 = -dA * (1 + eta2) * Math.Cos(F0);
            double g11 = 3 * dA * t * eta2 * Math.Cos(F0);
            double g03 = dA * (1 + t * t) * Math.Pow(Math.Cos(F0), 3) / 6;

            double h10 = dA * (1 - eta2 + eta2 * eta2) / Math.Cos(F0);          // Тука разлика с GPS + eta2*eta2
            double h20 = dA * t * (1 - eta2 / 2) / Math.Cos(F0);
            double h02 = -dA * t * Math.Cos(F0) / 2;
            double h30 = dA * (1 + 3 * t * t) / 3 / Math.Cos(F0);
            double h12 = -dA * (1 + t * t) * Math.Cos(F0) / 2;

            t = 1;
            while (t > 1e-10)                       // Итерации за връщане в първоначалната фиктивна точка
            {
                B = F1 - g01 * dL - g11 * dF * dL - g03 * dL * dL * dL;
                dF = B - F0;
                N = dL;
                L = L1 - h10 * dF - h20 * dF * dF - h02 * dL * dL - h30 * dF * dF * dF - h12 * dF * dL * dL;
                dL = L - L0;
                t = Math.Abs(N - dL);
            }
            // Географска ширина, географска дължина в градуси в КС1950
            B = B * 180 / _PI;                      // Географска ширина в ГРАДУСНА мярка!
            L = L * 180 / _PI;                      // Географска дължина

            return true;
        }

        // Трансформацията от проекционните координати - система „1950 г.” в 1942/83, 1942 & други неизвестни
        bool XY1950_XY4283(short fromto, ref double X, ref double Y)
        {
            ushort zone;
            if (Y >= 1e6)               // Номер на ивицата 4 или 5, две 6° зони – с осеви меридиани 21° и 27°
                zone = (ushort)(Y / 1e6);
            else
                return false;           // Номера на ивицата се определя от редукционното число!

            double dy, dx, dx2, x0_21, y0_21, x0_27, y0_27,
            a00_21, a10_21, a01_21, a20_21, a11_21, a12_21, a02_21, a30_21, a21_21, a03_21,
            b10_21, b02_21, b21_21, b01_21, b30_21, b12_21, b20_21, b11_21, b03_21, b00_21,
            a11_27, a20_27, a02_27, a01_27, a00_27, a10_27,
            b20_27, b11_27, b02_27, b01_27, b00_27, b10_27;

            double var_6, var_8;        //????

            dy = dx = dx2 = x0_21 = y0_21 = x0_27 = y0_27 =
            a00_21 = a10_21 = a01_21 = a20_21 = a11_21 = a12_21 = a02_21 = a30_21 = a21_21 = a03_21 =
            b10_21 = b02_21 = b21_21 = b01_21 = b30_21 = b12_21 = b20_21 = b11_21 = b03_21 = b00_21 =
            a11_27 = a20_27 = a02_27 = a01_27 = a00_27 = a10_27 =
            b20_27 = b11_27 = b02_27 = b01_27 = b00_27 = b10_27 = var_6 = var_8 = 0;

            switch (fromto)                     // Трансформация между - 
            {
                case 1:                             // 1942 -> 1942/83
                    a00_21 = 1.00092732646;         // ?
                    a10_21 = 1.13638402966;
                    a01_21 = 0.36432601477;
                    a20_21 = -0.02832148914;
                    a11_21 = -0.27972019802;
                    a02_21 = -0.2137788787;             //?
                    a30_21 = 0.0;                   // 
                    a21_21 = 0.0;
                    a12_21 = 0.0;
                    a03_21 = 0.0;                   //?
                    b00_21 = 8.1284444099e-1;
                    b10_21 = -2.4946768592e-1;
                    b01_21 = 1.11198258063;
                    b20_21 = 1.1246604972e-1;
                    b11_21 = 7.496228448e-2;
                    b02_21 = -8.64320147e-2;
                    b30_21 = 0.0;
                    b21_21 = 0.0;
                    b12_21 = 0.0;
                    b03_21 = 0.0;
                    x0_21 = 4.739248376e6;          // X0
                    y0_21 = 4.685727065e6;          // Y0

                    a00_27 = 1.80986360066;
                    a10_27 = 7.4309732877e-1;
                    a01_27 = 6.0034783331e-1;
                    a20_27 = -1.1014247434e-1;
                    a11_27 = -4.173611458e-2;
                    a02_27 = 1.2581534629e-1;
                    b00_27 = 2.80423763656;
                    b10_27 = -5.2505709353e-1;
                    b01_27 = 6.7880403968e-1;
                    b20_27 = 1.696063463e-2;
                    b11_27 = -2.0000615302e-1;
                    b02_27 = -2.96496844e-2;
                    x0_27 = 4.743832825e6;          // X0
                    y0_27 = 5.424655592e6;          // Y0
                    break;
                case 2:                             // 1942/83 -> 1942
                    a00_21 = -1.00092731185;
                    a10_21 = -1.1363721626;
                    a01_21 = -3.6431762342e-1;
                    a20_21 = 2.832172706e-2;
                    a11_21 = 2.7971199169e-1;
                    a02_21 = 2.1377093002e-1;
                    a30_21 = 0.0;
                    a21_21 = 0.0;
                    a12_21 = 0.0;
                    a03_21 = 0.0;
                    b00_21 = -8.1284509495e-1;
                    b10_21 = 2.4946208933e-1;
                    b01_21 = -1.11197160226;
                    b20_21 = -1.1246253975e-1;
                    b11_21 = -7.495808852e-2;
                    b02_21 = 8.642986298e-2;
                    b30_21 = 0.0;
                    b21_21 = 0.0;
                    b12_21 = 0.0;
                    b03_21 = 0.0;
                    x0_21 = 4.739249346e6;
                    y0_21 = 4.685727938e6;

                    a00_27 = -1.80986309331;
                    a10_27 = -7.4309518801e-1;
                    a01_27 = -6.0033938077e-1;
                    a20_27 = 1.1013990149e-1;
                    a11_27 = 4.173153837e-2;
                    a02_27 = -1.258132718e-1;
                    b00_27 = -2.80423696704;
                    b10_27 = 5.2504985086e-1;
                    b01_27 = -6.7880261238e-1;
                    b20_27 = -1.69585896e-2;
                    b11_27 = 2.0000245558e-1;
                    b02_27 = 2.9647268e-2;
                    x0_27 = 4.743834665e6;
                    y0_27 = 5.42465832e6;           // -> y0_27 ?
                    break;
                case 3:                             // 1942 -> 1950
                    a00_21 = -3.8282203417e-1;
                    a10_21 = -2.4279804742e-1;
                    a01_21 = 1.55522926076;
                    a20_21 = 3.01483584e-2;
                    a11_21 = 7.039030248e-2;
                    a02_21 = 2.184626707e-2;
                    a30_21 = 0.0;
                    a21_21 = 0.0;
                    a12_21 = 0.0;
                    a03_21 = 0.0;
                    b00_21 = -2.61741445721;
                    b10_21 = -1.4980953837;
                    b01_21 = -1.4070039741e-1;
                    b20_21 = -1.222392435e-2;
                    b11_21 = 2.6416728741e-1;
                    b02_21 = 6.3733766470e-2;
                    b30_21 = 0.0;
                    b21_21 = 0.0;
                    b12_21 = 0.0;
                    b03_21 = 0.0;
                    x0_21 = 4.739248376e6;
                    y0_21 = 4.685727065e6;

                    a00_27 = 3.00355729367;
                    a10_27 = 3.747734183e-2;
                    a01_27 = 1.3508271767;
                    a20_27 = 6.08452977e-2;
                    a11_27 = 2.3892012927e-1;
                    a02_27 = -1.458967164e-2;
                    b00_27 = -3.18069759013;
                    b10_27 = -1.33366570873;
                    b01_27 = -4.982468002e-2;
                    b20_27 = -8.653900218e-2;
                    b11_27 = 2.667952136e-2;
                    b02_27 = 1.6712836631e-1;
                    x0_27 = 4.744481023e6;
                    y0_27 = 5.424831078e6;          // Y0
                    break;
                case 4:                             //  1950 -> 1942 6°
                    a00_21 = 3.8282237767e-1;
                    a10_21 = 2.4277520208e-1;
                    a01_21 = -1.55523525968;
                    a20_21 = -3.014987009e-2;
                    a11_21 = -7.038576851e-2;
                    a02_21 = -2.184435047e-2;
                    a30_21 = 0.0;
                    a21_21 = 0.0;
                    a12_21 = 0.0;
                    a03_21 = 0.0;
                    b00_21 = 2.61741460149;
                    b10_21 = 1.49810104332;
                    b01_21 = 1.4067657572e-1;
                    b20_21 = 1.22197371e-2;
                    b11_21 = -2.6417275635e-1;
                    b02_21 = -6.373022233e-2;
                    b30_21 = 0.0;
                    b21_21 = 0.0;
                    b12_21 = 0.0;
                    b03_21 = 0.0;
                    x0_21 = 4.739248012e6;
                    y0_21 = 4.685724433e6;

                    a00_27 = -3.00355879964;
                    a10_27 = -3.74952315e-2;
                    a01_27 = -1.35082705431;
                    a20_27 = -6.084962835e-2;
                    a11_27 = -2.3891803924e-1;
                    a02_27 = 1.459544332e-2;
                    b00_27 = 3.18069867659;
                    b10_27 = 1.33366583636;
                    b01_27 = 4.980688626e-2;
                    b20_27 = 8.653821428e-2;
                    b11_27 = -2.668960323e-2;
                    b02_27 = -1.671279145e-1;
                    x0_27 = 4.744484126e6;          // X0 -> 27°
                    y0_27 = 5.424828002e6;          // Y0
                    break;
                case 5:                             // 1950 -> 1942/83
                    a00_21 = 1.33590208496;         // 21° -> X
                    a10_21 = 1.38149594859;         // 21° -> X
                    a01_21 = -1.18220346411;            // 21° -> X
                    a20_21 = -5.11735513e-2;            // 21° -> X
                    a11_21 = -3.3268632601e-1;      // 21° -> X
                    a02_21 = -2.4990695009e-1;      // 21° -> X
                    a30_21 = 0.0;
                    a21_21 = 0.0;
                    a12_21 = 0.0;
                    a03_21 = 0.0;
                    b00_21 = 3.36520137191;         // 21° -> Y
                    b10_21 = 1.23384886172;         // 21° -> Y
                    b01_21 = 1.25277862977;         // 21° -> Y
                    b20_21 = 1.363011985e-1;        // 21° -> Y
                    b11_21 = -1.6630564306e-1;      // 21° -> Y
                    b02_21 = -1.5096746064e-1;      // 21° -> Y
                    b30_21 = 0.0;                   // 21° -> Y
                    b21_21 = 0.0;                   // 21° -> Y
                    b12_21 = 0.0;                   // 21° -> Y
                    b03_21 = 0.0;                   // 21° -> Y
                    x0_21 = 4.735056026e6;          // 21° -> X0
                    y0_21 = 4.685018524e6;          // 21° -> Y0

                    a00_27 = -1.19321803534;            // 27 °
                    a10_27 = 7.0456442202e-1;
                    a01_27 = -7.4932427343e-1;
                    a20_27 = -1.7058052428e-1;
                    a11_27 = -2.8061858682e-1;
                    a02_27 = 1.4102587608e-1;
                    b00_27 = 5.9891074106;
                    b10_27 = 8.0625833452e-1;
                    b01_27 = 7.275154159e-1;
                    b20_27 = 9.927652412e-2;
                    b11_27 = -2.2599387448e-1;
                    b02_27 = -1.9831867652e-1;
                    x0_27 = 4.744294922e6;          // 27° X0
                    y0_27 = 5.425160279e6;          // 27° Y0 -> y0_27 !
                    break;
                case 6:                             // 1942/83 ->1950
                    a00_21 = -1.33590069761;
                    a10_21 = -1.3814916621;
                    a01_21 = 1.18217301986;
                    a20_21 = 5.116610429e-2;
                    a11_21 = 3.3267158632e-1;
                    a02_21 = 2.499039e-1;
                    a30_21 = 0.0;
                    a21_21 = 0.0;
                    a12_21 = 0.0;
                    a03_21 = 0.0;
                    b00_21 = -3.36520198319;
                    b10_21 = -1.23381644973;
                    b01_21 = -1.2527786915;
                    b20_21 = -1.3629838152e-1;
                    b11_21 = 1.662870226e-1;
                    b02_21 = 1.5096192126e-1;
                    b30_21 = 0.0;
                    b21_21 = 0.0;
                    b12_21 = 0.0;
                    b03_21 = 0.0;
                    x0_21 = 4.735057311e6;
                    y0_21 = 4.685021971e6;

                    a00_27 = 1.19321744877;
                    a10_27 = -7.0456496087e-1;
                    a01_27 = 7.493135572e-1;
                    a20_27 = 1.7057361045e-1;
                    a11_27 = 2.8061866377e-1;
                    a02_27 = -1.4101937626e-1;
                    b00_27 = -5.98910604494;
                    b10_27 = -8.0624600768e-1;
                    b01_27 = -7.2751571618e-1;
                    b20_27 = -9.927696976e-2;
                    b11_27 = 2.2598183631e-1;
                    b02_27 = 1.9831765311e-1;
                    x0_27 = 4.744293662e6;          // X0
                    y0_27 = 5.425166083e6;          // Y0
                    break;
                case 7:                             // ???
                    a00_21 = 6.3456530291223e2;
                    a10_21 = -5.819512465e-2;
                    a01_21 = -3.66363813002;
                    a20_21 = 1.1199380373e-1;
                    a11_21 = -3.2648117502e-1;
                    a02_21 = -8.0852777e-4;
                    a30_21 = 3.227992692e-2;
                    a21_21 = 4.740648207e-2;
                    a12_21 = -7.65560872e-2;
                    a03_21 = 2.52650706e-3;
                    b00_21 = -3.1253403281512e2;
                    b10_21 = 3.62916961778;
                    b01_21 = -1.3827344227e-1;
                    b20_21 = 2.0383412212e-1;
                    b11_21 = 1.7377036655e-1;
                    b02_21 = -1.3080575322e-1;
                    b30_21 = -8.7000908400e-3;
                    b21_21 = 1.3076892851e-1;
                    b12_21 = 1.2733950132e-1;
                    var_6 = 5.336795023e-2;     // ?????
                    b03_21 = 3.902580392e-2;
                    x0_21 = 4.788868908e6;
                    y0_21 = 4.506775314e6;
                    zone = 4;                       // Само за зона 4 с основен меридиан 21°
                    break;
                case 8:                             // ???
                    a00_21 = 6.3347949114314e2;
                    a10_21 = -1.65720961819;
                    a01_21 = -3.93836840318;
                    a20_21 = 6.845452044e-2;
                    a11_21 = -2.0419783245e-1;
                    a02_21 = 5.32248127e-3;
                    a30_21 = 2.80908683e-2;
                    a21_21 = 2.024508288e-2;
                    a12_21 = -5.200036234e-2;
                    a03_21 = 3.84358806e-3;
                    b00_21 = -3.1075557650338e2;
                    b10_21 = 3.91822456988;
                    b01_21 = -1.70527349832;
                    b20_21 = 1.3050522742e-1;
                    b11_21 = 1.0184176489e-1;
                    b02_21 = -7.722220494e-2;
                    b30_21 = -6.8593175e-4;
                    b21_21 = 1.0108756566e-1;
                    b12_21 = 8.081911302e-2;
                    var_6 = 3.902580392e-2;     // ?????
                    b03_21 = 3.902580392e-2;
                    x0_21 = 4.788868908e6;
                    y0_21 = 4.506775314e6;
                    zone = 4;                       // Само за зона 4 с основен меридиан 21°
                    break;
                case 9:                             // ???
                    a00_21 = -6.345652952836e2;
                    a10_21 = 5.806316737e-2;
                    a01_21 = 3.66364699649;
                    a20_21 = -1.120129171e-1;
                    a11_21 = 3.264668515e-1;
                    a02_21 = 8.23382230e-4;
                    a30_21 = -3.22786874e-2;
                    a21_21 = -4.74219559e-2;
                    a12_21 = 7.654859389e-2;
                    a03_21 = -2.52709395e-3;
                    b00_21 = 3.1253402445065e2;
                    b10_21 = -3.62917771138;
                    b01_21 = 1.3814223154e-1;
                    b20_21 = -2.0382332718e-1;
                    b11_21 = -1.7380527212e-1;
                    b02_21 = 1.307973959e-1;
                    b30_21 = 8.70655083e-3;
                    b21_21 = -1.307590073e-1;
                    b12_21 = -1.273471653e-1;
                    b03_21 = -5.337365764e-2;
                    x0_21 = 4.789503721e6;
                    y0_21 = 4.506462975e6;
                    zone = 4;                       // Само за зона 4 с основен меридиан 21°
                    break;
                case 10:                            // ???
                    a00_21 = -6.3347948340326e2;
                    a10_21 = 1.65708317743;
                    a01_21 = 3.93850123075;
                    a20_21 = -6.847088816e-2;
                    a11_21 = 2.0419854361e-1;
                    a02_21 = -5.31234768e-3;
                    a30_21 = -2.809214593e-2;
                    a21_21 = -2.025817773e-2;
                    a12_21 = 5.199940761e-2;
                    a03_21 = -3.84379737e-3;
                    b00_21 = 3.1075557319272e2;
                    b10_21 = -3.91835651529;
                    b01_21 = 1.70514917484;
                    b20_21 = -1.3050461995e-1;
                    b11_21 = -1.0187054229e-1;
                    b02_21 = 7.722086765e-2;
                    b30_21 = 6.9117902e-4;
                    b21_21 = -1.0108750374e-1;
                    b12_21 = -8.083014491e-2;
                    b03_21 = -3.903220301e-2;
                    x0_21 = 4.789502536e6;
                    y0_21 = 4.50646469e6;
                    zone = 4;                       // Само за зона 4 с основен меридиан 21°
                    break;
                case 11:                            // ???
                    a00_27 = 4.556193463e6;         // Афинна трансформация
                    a10_27 = 1.000008;
                    a01_27 = 6.836487e-6;
                    a20_27 = 0.0;
                    a11_27 = 0.0;
                    a02_27 = 0.0;
                    b00_27 = 5.4992404848e6;
                    b10_27 = -6.601964e-6;
                    b01_27 = 1.000008;
                    b20_27 = 0.0;
                    b11_27 = 0.0;
                    b02_27 = 0.0;
                    x0_27 = 4.556192608e6;          // X0
                    y0_27 = 5.499235986e6;          // Y0
                    zone = 5;                       // Само за зона 5 с основен меридиан 27°
                    break;
                default:
                    return false;                   // Връща непроменени координати
            }

            if (zone == 4)                      // Зона 4 за 6° с основен меридиан 21°
            {
                dx = (X - x0_21) * 1e-5;            // dx
                dy = (Y - y0_21) * 1e-5;            // dy
                dx2 = dx * dx;                  // dx^2

                X += ((a03_21 * dy + a12_21 * dx + a02_21) * dy + a21_21 * dx2 + a01_21) * dy + ((a30_21 * dx + a20_21) * dx + a11_21 * dy + a10_21) * dx + a00_21; // ?
                Y += ((b03_21 * dy + b12_21 * dx + b02_21) * dy + b21_21 * dx2 + b01_21) * dy + ((b30_21 * dx + b20_21) * dx + b11_21 * dy + b10_21) * dx + b00_21; // Ok

                return true;
            }
            else if (zone == 5)                  // Зона 5 за 6° с основен меридиан 27°
            {
                dx = (X - x0_27) * 1e-5;            // dx
                dy = (Y - y0_27) * 1e-5;            // dy var_YYY
                                                    //		dx2 = dx*dx;					// dx^2
                                                    //		var_8  = dx*(0);				// !
                                                    //		dx2 =(dy*(0)+var_8)*dy+dx*dx*(0);

                X += ((var_8 * dx + a20_27) * dx + a11_27 * dy + a10_27) * dx + (a02_27 * dy + dx2 + a01_27) * dy + a00_27;
                Y += ((var_8 * dx + b20_27) * dx + b11_27 * dy + b10_27) * dx + (b02_27 * dy + dx2 + b01_27) * dy + b00_27;

                return true;
            }

            return true;
        }
        bool XY1950_XY4283__(short fromto, ref double X, ref double Y)
        {
            ushort zone;
            if (Y >= 1e6)               // номер на ивицата 4 или 5, две 6° зони – с осеви меридиани 21° и 27°
                zone = (ushort)(Y / 1e6);
            else
                return false;           // Номера на ивицата се определя от редукционното число!

            double dy, dx, var_100, var_F8, var_F0, var_E8, var_E0,
            var_D8, var_D0, var_C8, var_C0, var_B8, var_B0, var_A8, var_A0, Y0_21,
            var_90, var_88, X0_27, var_78, Y0_27, var_68, var_60, var_58, var_50,
            var_48, var_40, var_38, var_30, var_28, var_20, var_18, dx2, var_8;

            double var_1xx, var_2xx, var_3xx, var_4xx, var_5xx, X0_21;  //????

            dy = dx = var_100 = var_F8 = var_F0 = var_E8 = var_E0 =
            var_D8 = var_D0 = var_C8 = var_C0 = var_B8 = var_B0 = var_A8 = var_A0 = Y0_21 =
            var_90 = var_88 = X0_27 = var_78 = Y0_27 = var_68 = var_60 = var_58 = var_50 =
            var_48 = var_40 = var_38 = var_30 = var_28 = var_20 = var_18 = dx2 = var_8 = 0;

            if (zone == 4)                      // 21
            {
                var_1xx = 1.33590208496;            // 21° -> X
                var_2xx = 1.38149594859;            // 21° -> X
                var_3xx = -1.18220346411;           // 21° -> X
                var_4xx = -5.11735513e-2;           // 21° -> X
                var_5xx = -3.3268632601e-1;     // 21° -> X
                var_E0 = -2.4990695009e-1;      // 21° -> X
                var_D0 = 0.0;
                var_B0 = 0.0;
                var_A0 = 0.0;
                var_F0 = 0.0;
                var_88 = 3.36520137191;         // 21° -> Y
                var_100 = 1.23384886172;            // 21° -> Y
                var_D8 = 1.25277862977;         // 21° -> Y
                var_B8 = 1.363011985e-1;        // 21° -> Y
                var_A8 = -1.6630564306e-1;      // 21° -> Y
                var_F8 = -1.5096746064e-1;      // 21° -> Y
                var_C8 = 0.0;                   // 21° -> Y
                var_E8 = 0.0;                   // 21° -> Y
                var_C0 = 0.0;                   // 21° -> Y
                var_90 = 0.0;                   // 21° -> Y
                X0_21 = 4.735056026e6;          // 21° -> X0
                Y0_21 = 4.685018524e6;          // 21° -> Y0

                dx = (X - X0_21) * 1e-5;            // dx
                dy = (Y - Y0_21) * 1e-5;            // dy
                dx2 = dx * dx;                  // dx^2

                X += fromto * (((var_F0 * dy + dx * var_A0 + var_E0) * dy + dx2 * var_B0 + var_3xx) * dy + ((dx * var_D0 + var_4xx) * dx + dy * var_5xx + var_2xx) * dx + var_1xx); // ?
                Y += fromto * (((var_90 * dy + dx * var_C0 + var_F8) * dy + dx2 * var_E8 + var_D8) * dy + ((dx * var_C8 + var_B8) * dx + dy * var_A8 + var_100) * dx + var_88); // Ok

                return true;
            }
            else if (zone == 5)                 // 27°
            {
                var_40 = -1.19321803534;            // 27 °
                var_30 = 7.0456442202e-1;
                var_18 = -7.4932427343e-1;
                var_20 = -1.7058052428e-1;
                var_60 = -2.8061858682e-1;
                var_50 = 1.4102587608e-1;
                var_28 = 5.9891074106;
                var_58 = 8.0625833452e-1;
                var_38 = 7.275154159e-1;
                var_78 = 9.927652412e-2;
                var_68 = -2.2599387448e-1;
                var_48 = -1.9831867652e-1;
                X0_27 = 4.744294922e6;          // 27° X0
                Y0_27 = 5.425160279e6;          // 27° Y0 -> Y0_27 !

                dx = (X - X0_27) * 1e-5;            // dx
                dy = (Y - Y0_27) * 1e-5;            // dy var_YYY
                dx2 = dx * dx;                      // dx^2
                var_8 = 0;
                dx2 = 0;
                X += fromto * (((var_8 + var_20) * dx + dy * var_60 + var_30) * dx + (dy * var_50 + dx2 + var_18) * dy + var_40);
                Y += fromto * (((var_8 + var_78) * dx + dy * var_68 + var_58) * dx + (dy * var_48 + dx2 + var_38) * dy + var_28);

                return true;
            }
            return false;
        }


        // Трансформация Гаус-Крюгерова проекция към Геодезически Географски координати и обратно 
        bool Gauss_XY_BL(ushort Ellipsoid, ushort bandwidth,    // ПРЕОБРАЗУВАНЕ НА НАПРЕЧНИ ЦИЛИНДРИЧНИ КООРДИНАТИ В ГЕОГРАФСКИ 
        ref double ScaleFactor, ref double Reduction5e5, ref double B, ref double L)   // Гаус-Крюгерова проекция към Геодезически Географски координати - Гаусова трансверзална проекция
        {
            // Проверка за коректност!!!
            double a, alfa, e2, ee2;                    // Константи на елипсоида
            a = Semi_major_axis[Ellipsoid];         // Екваториален радиус на Земята (голяма полуос)
            alfa = 1 / Inverse_flattening[Ellipsoid];   // Полярна сплеснатост на Земята
            e2 = 2 * alfa - alfa * alfa;                // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                        // Втори ексцентрицитет на втора степен - e'

            //	double c, m;							// Конвергенция и мащаб в точката
            int nom;                                // номер на ивицата
            double L0, be, Bf, en, Nf, etaf, Rs, St, p2, p4, p6, p8, tf;
            double X = B;                           // Координата X - Север
            double Y = L;                           // Координата Y - Изток
                                                    //	double ScaleFactor;						// Мащабен фактор
                                                    //	double Reduction5e5;					// Редукция по Y
                                                    //	ScaleFactor = 0.9996;					// Мащабен фактор за UTM = 0.9996, за Красовски = 1, за 1930 = 0.9999
                                                    //	Reduction5e5 = 5e5;						// Редукция по Y +500000
                                                    //	zona = 4;								// Номер на ивицата или зоната
                                                    //	bandwidth = 6;							// Ширина на зоната 3° или 6° ивица!
            nom = 0;                                // Номер на текуща зона
            L0 = B = L = 0;                         // Дължина на централен (осев) меридиан, резултатни географски координати
            if (bandwidth > 6)                      // Ако проекцията е UTM зоните са 34, 35, тогава широчината на ивицата е 6°
            {                                       // Без първите 6 зони в Тихия океан
                if (Y < 1e6)                        // Ако е Y не носи редукционно число на зоната
                {
                    Y += ((bandwidth <= 30) ? (bandwidth + 30) : (bandwidth - 30)) * 1e6;   // Прибавя 4 000 000 и 5 000 000, после се вади
                }
                bandwidth = 6;                      // Ширина на ивицата 6°
            }
            switch (bandwidth)                      // В зависимост от широчината на ивицата 3° или 6°
            {
                case 3:                                 // Ширина на ивицата 3°
                    nom = (Y >= 1e6) ? (int)(Y / 1e6) : 0;    // Номер на 3° зона или Гринуички меридиан
                    Y = Y - nom * 1e6 - Reduction5e5;       // Премахват се редукциите
                    L0 = nom * 3;                           // Дължина на централен меридиан Определяне на осевия меридиан в зависимост от номера на зоната
                    break;
                case 6:                                 // Ширина на ивицата 6°
                    if (Y > 1e6)                        // Редукия с номера на ивицата
                    {
                        nom = (int)(Y / 1e6);             // Номер на 6° зона или Гринуички меридиан
                        Y = Y - nom * 1e6 - Reduction5e5;   // Премахват се редукциите
                        L0 = 6 * nom - 3;                   // Дължина на централен меридиан Определяне на осевия меридиан в зависимост от номера на зоната
                    }
                    else                                // или Гринуички меридиан
                    {
                        Y = Y - Reduction5e5;           // Премахват се редукциите
                        L0 = 3;                         // Дължина на централен меридиан Определяне на осевия меридиан в зависимост от номера на зоната
                    }
                    break;
                default:
                    return false;                       // дали да не продължи с 3° или 6°?
            }

            L0 = L0 * _PI / 180;                            // Дължина на Основен меридиан в РАДИАНИ
            Y /= ScaleFactor;                       // Премахва се мащабния фактор по осите
            X /= ScaleFactor;

            be = a * Math.Sqrt(1 - e2);
            Rs = a * (1 - e2 / 4 - e2 * e2 * 3 / 64 - e2 * e2 * e2 * 5 / 256);
            St = X / Rs;
            en = (a - be) / (a + be);
            p2 = en * 3 / 2 - en * en * en * 27 / 32 + en * en * en * en * en * 269 / 512;
            p4 = en * en * 21 / 16 - en * en * en * en * 55 / 32;
            p6 = en * en * en * 151 / 96;
            p8 = en * en * en * en * 1092 / 512;
            Bf = St + (p2 * Math.Sin(2 * St) + p4 * Math.Sin(4 * St) + p6 * Math.Sin(6 * St) + p8 * Math.Sin(8 * St));  // Изометрична ширина
            Nf = a / Math.Sqrt(1 - e2 * Math.Sin(Bf) * Math.Sin(Bf));   // Напречен радиус на кривина на референтния елипсоид
            etaf = Math.Sqrt((a * a - be * be) / (be * be)) * Math.Cos(Bf);
            tf = Math.Tan(Bf);

            p2 = Math.Pow(Y, 2) * tf * (-1 - etaf * etaf) / (2 * Nf * Nf);
            p4 = Math.Pow(Y, 4) * tf * (5 + 3 * tf * tf + 6 * etaf * etaf - 6 * tf * tf * etaf * etaf - 3 * Math.Pow(etaf, 4) - 9 * tf * tf * Math.Pow(etaf, 4)) / (24 * Math.Pow(Nf, 4));
            p6 = Math.Pow(Y, 6) * tf * (-61 - 90 * tf * tf - 45 * Math.Pow(tf, 4) - 107 * Math.Pow(etaf, 2) + 162 * tf * tf * etaf * etaf + 45 * Math.Pow(tf, 4) * etaf * etaf) / (720 * Math.Pow(Nf, 6));
            p8 = Math.Pow(Y, 8) * tf * (1385 + 3633 * tf * tf + 4095 * Math.Pow(tf, 4) + 1575 * Math.Pow(tf, 6)) / 40320 / Math.Pow(Nf, 8);
            B = Bf + p2 + p4 + p6 + p8;             // Географска ширина

            p2 = Y / (Nf * Math.Cos(Bf));
            p4 = Math.Pow(Y, 3) * (-1 - 2 * tf * tf - etaf * etaf) / (6 * Nf * Nf * Nf * Math.Cos(Bf));
            p6 = Math.Pow(Y, 5) * (5 + 28 * tf * tf + 24 * tf * tf * tf * tf + 6 * etaf * etaf + 8 * tf * tf * etaf * etaf) / (120 * Nf * Nf * Nf * Nf * Nf * Math.Cos(Bf));
            p8 = Math.Pow(Y, 7) * (-61 - 662 * tf * tf - 1320 * Math.Pow(tf, 4) - 720 * Math.Pow(tf, 6)) / (5040 * Math.Pow(Nf, 7) * Math.Cos(Bf));
            L = L0 + p2 + p4 + p6 + p8;             // Географска дължина
                                                    //	if (L < 0)								// На запад от Гринуичкия меридиан
                                                    //		L+= _2PI;
            B = B * 180 / _PI;                          // Географска ширина В ГРАДУСНА мярка!
            L = L * 180 / _PI;                          // Географска дължина в ГРАДУСНА мярка

            return true;
        }

        bool Gauss_BL_XY(ushort Ellipsoid, ushort bandwidth, ref short currentZone,// ПРЕОБРАЗУВАНЕ НА ГЕОГРАФСКИ В НАПРЕЧНИ ЦИЛИНДРИЧНИ КООРДИНАТИ
        ref double ScaleFactor, ref double Reduction5e5, ref double X, ref double Y)        // Геодезически географски координати към Гаус-Крюгерова проекция - Гаусова трансверзална проекция
        {
            if (X < 0 || X > 90) return false;      // Ако географските координати са извън границите! с ASSERT?
            if (Y < 0 || Y > 360) return false;
            double a, alfa, e2, ee2;                    // Константи на елипсоида
            a = Semi_major_axis[Ellipsoid];         // Екваториален радиус на Земята (голяма полуос)
            alfa = 1 / Inverse_flattening[Ellipsoid];   // Полярна сплеснатост на Земята
            e2 = 2 * alfa - alfa * alfa;                    // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                        // Втори ексцентрицитет на втора степен - e'

            short nn, utmZone;             // номер на ивицата
            double m = Reduction5e5;                // Конвергенция и мащаб (Заради const прехвърлям редукцията)
            double L0, t, eta, N, S, S1, S2, S3, S4, S5, ll;
            double B = X;                           // Географска ширина
            double L = Y;                           // Географска дължина
                                                    //	a=6378137.000;							// Екваториален радиус на Земята (голяма полуос) за GRS 1980
                                                    //	alfa=1/298.257222101;					// Полярна сплеснатост на Земята за GRS 1980
                                                    //	ScaleFactor = 0.9996;					// Мащабен фактор за UTM = 0.9996, за Красовски = 1.
                                                    //	Reduction5e5 = 5e5;						// Редукция по Y +500000
                                                    //	zona = 4;								// Номер на ивицата или зоната
                                                    //	bandwidth = 3;							// Ширина на зоната 3° или 6° ивица! 34 и 35 за UTM проекция
                                                    //	currentZone = 0							// Коефициент за редукция на основен меридиан 0-текущ, -1 предходен, +1 следващ
            L0 = X = Y = 0;                         // Основен меридиан на ивицата - би трябвало да се задава произволен!
            utmZone = 0;
            if (bandwidth > 6)                      // Широчина на ивицата 3°, 6°. Ако е по-голяма от 6 тогава UTM зона
            {
                utmZone = (short)(bandwidth > 30 ? bandwidth - 30 : bandwidth + 30);    // Цялата Земя без първите 6 зони в Тихия океан!
                                                                                        //		utmZone = bandwidth - 30;			// За Б-я 34, 35, така само за Източното полукълбо
                bandwidth = 6;                      // За UTM проекция широчината на ивицата е 6°
                currentZone = 0;                    // Текущата зона ще се определя от зададената зона в широчината
            }
            //	cout<<"currentZone = "<<currentZone<<endl;
            switch (bandwidth)                      // Широчина на ивицата 3° или 6°
            {
                case 3:                                 // Определя 3° ивица
                    nn = (short)((L + 1.5) / 3);                // номер на 3° текуща ивицата
                    if (Math.Abs(currentZone) > 2)           // Ако е подаден основен меридиан
                        nn = (short)((currentZone + 1.5) / 3);  // номер на 3° текуща ивицата
                    else
                    {
                        nn += currentZone;              // currentZone = 0;+1;-1 за текуща ивица или съседна
                        if (nn <= 0) nn += 120;         // 120 основни меридиана
                    }
                    L0 = nn * 3;                            // Дължина на Основен меридиан  -> 24° или 27° за България
                    m += nn * 1e6;                      // Редукция по Y 500 000 + N*1 000 000
                    break;
                case 6:                                 // Определя 6° ивица
                    if (utmZone == 0)                   // Гаус-Крюгерова проекция
                    {
                        nn = (short)((L / 6) + 1);                // Номер на 6° текуща ивица
                        if (Math.Abs(currentZone) > 2)       // Ако е подаден основен меридиан
                            nn = (short)((L / 6) + 1);            // Номер на 6° текуща ивицата
                        else
                            nn += currentZone;          // currentZone = 0;+1;-1 за текуща ивица или съседна

                        if (nn < 1) nn += 60;           // 60 основни меридиана
                        L0 = 6 * nn - 3;                    // Дължина на Основен меридиан  -> 21° или 27° за България
                        m += nn * 1e6;                  // Редукция по Y + 500 000 + N*1 000 000
                    }
                    else                                // Ако е UTM проекция, ос Y няма редукция N*1 000 000
                    {
                        L0 = 6 * utmZone - 3;               // Дължина на Основен меридиан -> 21° или 27° или от зони 34 или 35 -> 4 или 5 за България
                    }
                    break;
                default:
                    return false;                       // дали да не продължи с 3° или 6°?
            }

            B = B * _PI / 180;                          // Географска ширина в РАДИАНИ 
            L = L * _PI / 180;                          // Географска дължина в РАДИАНИ 
            L0 = L0 * _PI / 180;                            // Дължина на Основен меридиан в РАДИАНИ

            S1 = a * (1 - e2) * (1 + e2 * 3 / 4 + e2 * e2 * 45 / 64 + e2 * e2 * e2 * 350 / 512 + e2 * e2 * e2 * e2 * 11025 / 16384) * B;
            S2 = -a * (1 - e2) * (e2 * 3 / 4 + e2 * e2 * 60 / 64 + e2 * e2 * e2 * 525 / 512 + e2 * e2 * e2 * e2 * 17640 / 16384) * Math.Sin(2 * B) / 2;
            S3 = a * (1 - e2) * (e2 * e2 * 15 / 64 + e2 * e2 * e2 * 210 / 512 + e2 * e2 * e2 * e2 * 8820 / 16384) * Math.Sin(4 * B) / 4;
            S4 = -a * (1 - e2) * (e2 * e2 * e2 * 35 / 512 + e2 * e2 * e2 * e2 * 2520 / 16384) * Math.Sin(6 * B) / 6;
            S5 = a * (1 - e2) * (e2 * e2 * e2 * e2 * 315 / 16384) * Math.Sin(8 * B) / 8;
            S = S1 + S2 + S3 + S4 + S5;             // Географска ширина на дъга по меридиана (B(f))
            N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B), 2));       // Напречен радиус на кривина на референтния елипсоид
            t = Math.Tan(B);                            //
            eta = Math.Cos(B) * Math.Sqrt(e2 / (1 - e2));           //
        lb: ll = L - L0;                                // Разлика в дължините от централен меридиан

            S1 = S + N * Math.Cos(B) * Math.Cos(B) * t * ll * ll / 2;
            S2 = N * Math.Pow(Math.Cos(B), 4) * t * (5 - t * t + 9 * eta * eta + 4 * Math.Pow(eta, 4)) * Math.Pow(ll, 4) / 24;
            S3 = N * Math.Pow(Math.Cos(B), 6) * t * (61 - 58 * t * t + t * t * t * t + 270 * eta * eta - 330 * t * t * eta * eta) * Math.Pow(ll, 6) / 720;
            S4 = N * Math.Pow(Math.Cos(B), 8) * t * (1385 - 3111 * t * t + 543 * t * t * t * t - Math.Pow(t, 6)) * Math.Pow(ll, 8) / 40320;
            X = ScaleFactor * (S1 + S2 + S3 + S4);          // X координата на север мащабирани с мащабен коефициент

            S1 = N * Math.Cos(B) * ll;
            S2 = N * Math.Pow(Math.Cos(B), 3) * (1 - t * t + eta * eta) * ll * ll * ll / 6;
            S3 = N * Math.Pow(Math.Cos(B), 5) * (5 - 18 * t * t + t * t * t * t + 14 * eta * eta - 58 * t * t * eta * eta) * Math.Pow(ll, 5) / 120;
            S4 = N * Math.Pow(Math.Cos(B), 7) * (61 - 479 * t * t + 179 * t * t * t * t - Math.Pow(t, 6)) * Math.Pow(ll, 7) / 5040;
            Y = ScaleFactor * (S1 + S2 + S3 + S4);          // Y координата без редукцията
            if (Y + Reduction5e5 < 0)               // Автоматични преминава в съседна зона
            {                                       // Трябва да мина една зона на запад
                L0 -= bandwidth * _PI / 180;                // Минава в съседна западна зона
                m -= 1e6;                           // Намаля редукцията с 1e6
                if (L0 < 0)
                {
                    L0 += _2PI;
                    m = 360 / bandwidth;                // 120 или 60 зона
                }
                goto lb;
            }
            else if (Y + Reduction5e5 >= 1e6)       //
            {                                       // Трябва да мина една зона на изток
                L0 += bandwidth * _PI / 180;                // Минава в съседна източна зона
                m += 1e6;                           // Увеличава редукцията с 1 000 000
                if (L0 > _2PI)
                {
                    L0 -= _2PI;                     //
                    m = 1;                          // 1 зона
                }
                goto lb;
            }
            else
                Y += m;                             // Y координата на изток + редукция 500 000!
            return true;
        }

        // Трансформация Конична конформна Ламбертова проекция с два стандартни паралела
        bool Lambert2_BL_XY(ushort Ellipsoid, ref double L0,           //  Конична конформна Ламбертова проекция с два стандартни паралела
         ref double B1, ref double B2, ref double Y0, ref double X, ref double Y)     // КАДАСТРАЛНА КООРДИНАТНА СИСТЕМА 2005
        {
            double a, alfa, e, e2;                  // Константи на елипсоида
            a = Semi_major_axis[Ellipsoid];         // Екваториален радиус на Земята (голяма полуос)
            alfa = 1 / Inverse_flattening[Ellipsoid];   // Полярна сплеснатост на Земята
                                                        //	a = 6378245.;							// Красовски 1942
                                                        //	alfa = 1/298.3;							// Красовски 1942
            e2 = 2 * alfa - alfa * alfa;                    // Първи ексцентрицитет на втора степен - e
            e = Math.Sqrt(e2);                          // Първи ексцентрицитет - e
                                                        //	ee2 = e2/(1 - e2);						// Втори ексцентрицитет на втора степен - e'

            L0 = L0 * _PI / 180;                        // Централен меридиан: 25°30'00.0000" в РАДИАНИ
            B1 = B1 * _PI / 180;                        // Първи стандартен паралел: 42°00’00”	//41°51'11.2153" в РАДИАНИ
            B2 = B2 * _PI / 180;                        // Втори стандартен паралел: 43°20’00”	//43°28'35.8786" в РАДИАНИ

            double B = X * _PI / 180;                   // Географска ширина в РАДИАНИ
            double L = Y * _PI / 180;                   // географска дължина в РАДИАНИ

            double w1, w2, w0, Q1, Q2, Q0;
            w1 = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B1), 2));
            w2 = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B2), 2));
            Q1 = (Math.Log((1.0 + Math.Sin(B1)) / (1.0 - Math.Sin(B1))) - e * Math.Log((1.0 + e * Math.Sin(B1)) / (1.0 - e * Math.Sin(B1)))) / 2;   // изометричната ширина 
            Q2 = (Math.Log((1.0 + Math.Sin(B2)) / (1.0 - Math.Sin(B2))) - e * Math.Log((1.0 + e * Math.Sin(B2)) / (1.0 - e * Math.Sin(B2)))) / 2;   // изометричната ширина 
            double B0 = Math.Asin(Math.Log(w2 * Math.Cos(B1) / w1 / Math.Cos(B2)) / (Q2 - Q1));   //
                                                                                                  //	cout << B0 << endl;
            Q0 = (Math.Log((1.0 + Math.Sin(B0)) / (1.0 - Math.Sin(B0))) - e * Math.Log((1.0 + e * Math.Sin(B0)) / (1.0 - e * Math.Sin(B0)))) / 2;   // ширина на централния паралел 
            w0 = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B0), 2));

            double Re;      // радиус на образа на екватора, приет за начало на ширините 
                            //	Re = a*cos(B0)*exp(Q0*sin(B0))/w0/sin(B0);		// !!!
            Re = a * Math.Cos(B1) * Math.Exp(Q1 * Math.Sin(B0)) / w1 / Math.Sin(B0);        // !!!
                                                                                            //	Re = a*cos(B2)*exp(Q2*sin(B0))/w2/sin(B0);		// !!!
            double R0;      // радиус на образа на централния паралел
            R0 = Re / Math.Exp(Q0 * Math.Sin(B0));

            double m0, m2, m4, m6, m8;
            m0 = a * (1 - e2);
            m2 = 1.5 * e2 * m0;
            m4 = 1.25 * e2 * m2;
            m6 = 7.0 / 6.0 * e2 * m4;
            m8 = 1.125 * e2 * m6;
            double a0, a2, a4, a6;
            a0 = m0 + 0.5 * m2 + 0.375 * m4 + 0.3125 * m6 + 0.2734375 * m8;
            a2 = 0.5 * m2 + 0.5 * m4 + 0.46875 * m6 + 0.4375 * m8;
            a4 = 0.125 * m4 + 0.1875 * m6 + 0.21875 * m8;
            a6 = 0.03125 * m6 + 0.0625 * m8;
            double x0;                      // абциса на централната точка на проекцията
            x0 = a0 * B0 - Math.Sin(B0) * Math.Cos(B0) * (a2 - a4 + a6 + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B0), 2) + 16 * a6 * Math.Pow(Math.Sin(B0), 4) / 3);
            //	cout<<endl<<"X0="<<x0<<endl;
            double R;
            Q0 = (Math.Log((1.0 + Math.Sin(B)) / (1.0 - Math.Sin(B))) - e * Math.Log((1.0 + e * Math.Sin(B)) / (1.0 - e * Math.Sin(B)))) / 2;   // ширина на дадения паралел
            R = Re / Math.Exp(Q0 * Math.Sin(B0));         // определяне на радиуса на образа на текущия паралел 
            m0 = (L - L0) * Math.Sin(B0);            // определяне на меридианната конвергенция

            X = R0 + x0 - R * Math.Cos(m0);      // Редукция + север
            Y = Y0 + R * Math.Sin(m0);               // Редукция + изток
            return true;
        }

        bool Lambert2_XY_BL(ushort Ellipsoid, ref double L0,           //  Конична конформна Ламбертова проекция с два стандартни паралела
            ref double B1, ref double B2, ref double Y0, ref double B, ref double L)      //  КАДАСТРАЛНА КООРДИНАТНА СИСТЕМА 2005
        {
            double a, alfa, e, e2;              // Константи на елипсоида
            a = Semi_major_axis[Ellipsoid];         // Екваториален радиус на Земята (голяма полуос)
            alfa = 1 / Inverse_flattening[Ellipsoid];   // Полярна сплеснатост на Земята
                                                        //	a = 6378245.;						// Красовски 1942
                                                        //	alfa = 1/298.3;						// Красовски 1942
            e2 = 2 * alfa - alfa * alfa;                // Първи ексцентрицитет на втора степен - e
            e = Math.Sqrt(e2);                      // Първи ексцентрицитет - e
                                                    //	ee2 = e2/(1 - e2);					// Втори ексцентрицитет на втора степен - e'

            L0 = L0 * _PI / 180;                    // Централен меридиан: 25°30'00.0000" в РАДИАНИ
            B1 = B1 * _PI / 180;                    // Първи стандартен паралел: 42°00’00”	//41°51'11.2153" в РАДИАНИ
            B2 = B2 * _PI / 180;                    // Втори стандартен паралел: 43°20’00”	//43°28'35.8786" в РАДИАНИ

            double X = B;                       // координата X-север, в метрична мярка
            double Y = L;                       // координата Y-изток, в метрична мярка

            double w1, w2, w0, Q1, Q2, Q0;
            w1 = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B1), 2));
            w2 = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B2), 2));
            Q1 = (Math.Log((1.0 + Math.Sin(B1)) / (1.0 - Math.Sin(B1))) - e * Math.Log((1.0 + e * Math.Sin(B1)) / (1.0 - e * Math.Sin(B1)))) / 2;     // изометричната ширина 
            Q2 = (Math.Log((1.0 + Math.Sin(B2)) / (1.0 - Math.Sin(B2))) - e * Math.Log((1.0 + e * Math.Sin(B2)) / (1.0 - e * Math.Sin(B2)))) / 2;     // изометричната ширина 
            double B0 = Math.Asin(Math.Log(w2 * Math.Cos(B1) / w1 / Math.Cos(B2)) / (Q2 - Q1));

            Q0 = (Math.Log((1.0 + Math.Sin(B0)) / (1.0 - Math.Sin(B0))) - e * Math.Log((1.0 + e * Math.Sin(B0)) / (1.0 - e * Math.Sin(B0)))) / 2;   // ширина на централния паралел 
            w0 = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B0), 2));

            double Re; // радиус на образа на екватора, приет за начало на ширините 
                       //	Re = a*cos(B0)*exp(Q0*sin(B0))/w0/sin(B0);		// не е вярно
            Re = a * Math.Cos(B1) * Math.Exp(Q1 * Math.Sin(B0)) / w1 / Math.Sin(B0);        // !!!
                                                                                            //	Re = a*cos(B2)*exp(Q2*sin(B0))/w2/sin(B0);		// !!!
            double R0;  // радиус на образа на централния паралел
            R0 = Re / Math.Exp(Q0 * Math.Sin(B0));

            double m0, m2, m4, m6, m8;
            m0 = a * (1 - e2);
            m2 = 1.5 * e2 * m0;
            m4 = 1.25 * e2 * m2;
            m6 = 7.0 / 6.0 * e2 * m4;
            m8 = 1.125 * e2 * m6;
            double a0, a2, a4, a6;
            a0 = m0 + 0.5 * m2 + 0.375 * m4 + 0.3125 * m6 + 0.2734375 * m8;
            a2 = 0.5 * m2 + 0.5 * m4 + 0.46875 * m6 + 0.4375 * m8;
            a4 = 0.125 * m4 + 0.1875 * m6 + 0.21875 * m8;
            a6 = 0.03125 * m6 + 0.0625 * m8;
            double x0;
            x0 = a0 * B0 - Math.Sin(B0) * Math.Cos(B0) * (a2 - a4 + a6 + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B0), 2) + 16 * a6 * Math.Pow(Math.Sin(B0), 4) / 3);
            double R;
            R = Math.Sqrt(Math.Pow((Y - Y0), 2) + Math.Pow((R0 + x0 - X), 2));
            Q0 = Math.Log(Re / R) / Math.Sin(B0);
            m0 = Math.Asin((Math.Exp(2 * Q0) - 1) / (Math.Exp(2 * Q0) + 1));
            B = 0;
            while (Math.Abs(B - m0) > 1.0e-12)              // Итерация за определяна на ширината
            {
                m0 = B;
                w1 = (Math.Log((1.0 + Math.Sin(m0)) / (1.0 - Math.Sin(m0))) - e * Math.Log((1.0 + e * Math.Sin(m0)) / (1.0 - e * Math.Sin(m0)))) / 2 - Q0;
                w2 = 1 / (1 - Math.Pow(Math.Sin(m0), 2)) - e2 / (1 - e2 * Math.Pow(Math.Sin(m0), 2));
                B = Math.Asin(Math.Sin(m0) - w1 / w2);                // Географска ширина в РАДИАНИ
            }

            m0 = Math.Atan((Y - Y0) / (R0 + x0 - X));
            L = m0 / Math.Sin(B0) + L0;                      // географска дължина в РАДИАНИ

            B = B * 180 / _PI;                              // Географска ширина в ГРАДУСНА мярка!
            L = L * 180 / _PI;                              // Географска дължина в градуси

            return true;
        }

        // Трансформация на геодезически географски координати (B, L, h) в правоъгълни пространствени координати (X, Y, Z)
        bool BLH_XYZ(ushort Ellipsoide, ref double X, ref double Y, ref double Z)   // Трансформация на геодезически географски координати (B, L, h) в правоъгълни пространствени координати (X, Y, Z)
        {
            double a, alfa, N, e2;                  // Константи на елипсоида
            a = Semi_major_axis[Ellipsoide];            // Екваториален радиус на Земята (голяма полуос) на референтен елипсоид
            alfa = 1 / Inverse_flattening[Ellipsoide];  // Полярна сплеснатост на Земята
            e2 = 2 * alfa - alfa * alfa;                    // Първи ексцентрицитет на втора степен - e

            double B = X * _PI / 180;                   // Географска ширина в РАДИАНИ
            double L = Y * _PI / 180;                   // географска дължина в РАДИАНИ
            double h = Z;                           // Наделипсоидна височина (в метри) h - Височина над елипсоида, мерена по нормалата (h = H + ондулацията (аномалия))

            N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B), 2));       // Напречен радиус на кривина на референтния елипсоид
            X = (N + h) * Math.Cos(B) * Math.Cos(L);            // Пространствени Декартови координати
            Y = (N + h) * Math.Cos(B) * Math.Sin(L);            //
            Z = (N * (1 - e2) + h) * Math.Sin(B);           //

            return true;
        }

        bool XYZ_BLH(ushort Ellipsoide, ref double B, ref double L, ref double h)   // Трансформация на правоъгълни пространствени координати (X, Y, Z) в геодезически географски координати (B, L, h)
        {
            double a, alfa, b, p, q, N, e2, ee2;            // Константи на елипсоида
            a = Semi_major_axis[Ellipsoide];                // Екваториален радиус на Земята (голяма полуос) за референтен елипсоид
            alfa = 1 / Inverse_flattening[Ellipsoide];      // Полярна сплеснатост на Земята за референтен елипсоид
            e2 = 2 * alfa - alfa * alfa;                        // Първи ексцентрицитет на втора степен - e
            ee2 = e2 / (1 - e2);                            // Втори ексцентрицитет на втора степен - e'
            b = a * (1 - alfa);                         // Малка полуос 

            double X = B;                               // Пространствени Декартови координати X
            double Y = L;                               // Пространствени Декартови координати Y
            double Z = h;                               // Пространствени Декартови координати Z

            L = Math.Atan(Y / X);                                // Географска дължина в радиани
            p = Math.Sqrt(X * X + Y * Y);
            q = Math.Atan(Z * a / p / b);
            B = Math.Atan((Z + ee2 * b * Math.Pow(Math.Sin(q), 3)) / (p - e2 * a * Math.Pow(Math.Cos(q), 3))); // Географска ширина в радиани
            N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(B), 2));       // Напречен радиус на кривина на референтния елипсоид

            h = p / Math.Cos(B) - N;                // h - Височина над елипсоида, мерена по нормалата (h = H + ондулацията (аномалия))

            B = B * 180 / _PI;                              // В ГРАДУСИ Географска ширина
            L = L * 180 / _PI;                              // В Градуси Географска дължина

            return true;
        }

        // Трансформация между елипсоид Красовски 1942/83 и WGS84 епоха 2005
        bool XYZ4283_XYZ2005(ref double X, ref double Y, ref double Z)  // КОНФОРМНА ТРАНСФОРМАЦИЯ МЕЖДУ ПРОСТРАНСТВЕНИ ДЕКАРТОВИ КООРДИНАТНИ СИСТЕМИ ТРАНСФОРМАЦИОННИ ПАРАМЕТРИ МЕЖДУ СИСТЕМА „1942/83 Г.” И БГС2005
        {
            double Xm = 4223032.260;        // Централна точка
            double Ym = 2032777.529;
            double Zm = 4309209.044;
            double dX = 5.24930;            // Транслация м
            double dY = -132.343;           // Транслация м
            double dZ = -103.9394;          // Транслация м
            double Rx = -1.370062 * _PI / 648000.0;//=3600./180.;	// Ротация " -> радиани
            double Ry = -2.006591 * _PI / 648000.0;//=3600./180.;	// Ротация " -> радиани
            double Rz = 3.409304 * _PI / 648000.0;//=3600./180.;	// Ротация " -> радиани
            double m = 1.0 / 1.0000039901;        // Мащабен коефициент
            double dx, dy, dz;

            dx = X - Xm;
            dy = Y - Ym;
            dz = Z - Zm;
            X = (dx + Rz * dy - Ry * dz) * m + dX + Xm;
            Y = (-Rz * dx + dy + Rx * dz) * m + dY + Ym;
            Z = (+Ry * dx - Rx * dy + dz) * m + dZ + Zm;

            return true;
        }

        bool XYZ2005_XYZ4283(ref double X, ref double Y, ref double Z)  // КОНФОРМНА ТРАНСФОРМАЦИЯ МЕЖДУ ПРОСТРАНСТВЕНИ ДЕКАРТОВИ КООРДИНАТНИ СИСТЕМИ ТРАНСФОРМАЦИОННИ ПАРАМЕТРИ МЕЖДУ БГС2005 И СИСТЕМА „1942/83 Г.”
        {
            double Xm = 4223032.260;        // Централна точка
            double Ym = 2032777.529;
            double Zm = 4309209.044;
            double dX = -5.249300;          // Транслация м
            double dY = 1.32343e2;          // Транслация м
            double dZ = 1.039394e2;         // Транслация м
            double Rx = 1.370062 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double Ry = 2.006591 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double Rz = -3.409304 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double m = 1.0 + 3.9901e-6;       // Мащабен коефициент
            double dx, dy, dz;

            dx = X - Xm;
            dy = Y - Ym;
            dz = Z - Zm;
            X = (dx + Rz * dy - Ry * dz) * m + dX + Xm;
            Y = (-Rz * dx + dy + Rx * dz) * m + dY + Ym;
            Z = (+Ry * dx - Rx * dy + dz) * m + dZ + Zm;

            return true;
        }


        // BulRef1989 - WGS84
        bool XYZ89_XYZ2005(ref double X, ref double Y, ref double Z)
        {
            double Xm = 4.232496704e6;      // Централна точка
            double Ym = 1.993413221e6;
            double Zm = 4.316918266e6;
            double dX = 6.58e-2;            // Транслация м
            double dY = -2.46e-2;           // Транслация м
            double dZ = -5.27e-2;           // Транслация м
            double Rx = 2.3e-4 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double Ry = -2.43e-3 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double Rz = 1.506e-3 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double m = 1.0 - 1.16e-8;           // Мащабен коефициент
            double dx, dy, dz;

            dx = X - Xm;
            dy = Y - Ym;
            dz = Z - Zm;
            X = (dx + Rz * dy - Ry * dz) * m + dX + Xm;
            Y = (-Rz * dx + dy + Rx * dz) * m + dY + Ym;
            Z = (+Ry * dx - Rx * dy + dz) * m + dZ + Zm;
            return true;
        }

        bool XYZ2005_XYZ89(ref double X, ref double Y, ref double Z)
        {
            double Xm = 4.232496704e6;      // Централна точка
            double Ym = 1.993413221e6;
            double Zm = 4.316918266e6;
            double dX = -6.58e-2;           // Транслация м
            double dY = 2.46e-2;            // Транслация м
            double dZ = 5.27e-2;            // Транслация м
            double Rx = -2.3e-4 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double Ry = 2.43e-3 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double Rz = -1.506e-3 * _PI / 648000.0;//=3600/180;	// Ротация " -> радиани
            double m = 1 / (1.0 - 1.16e-8);         // Мащабен коефициент
            double dx, dy, dz;

            dx = X - Xm;
            dy = Y - Ym;
            dz = Z - Zm;
            X = (dx + Rz * dy - Ry * dz) * m + dX + Xm;
            Y = (-Rz * dx + dy + Rx * dz) * m + dY + Ym;
            Z = (+Ry * dx - Rx * dy + dz) * m + dZ + Zm;
            return true;
        }


        // BulRef1989 - 1942/83
        bool XYZ89_XYZ4283(ref double X, ref double Y, ref double Z)
        {                               // Трансформация по Молоденски-Бадекас -Bulref89 - KS1942/83
            double Xm = 4188742.631;        // Централна точка
            double Ym = 2016041.319;
            double Zm = 4349746.951;
            double dX = -5.426;             // Транслация м
            double dY = 131.928;            // Транслация м
            double dZ = 103.833;            // Транслация м
            double Rx = 1.363718 * _PI / 648000.0;//=3600./180;	// Ротация " -> радиани
            double Ry = 1.980293 * _PI / 648000.0;//=3600./180;	// Ротация " -> радиани
            double Rz = -3.422231 * _PI / 648000.0;//=3600./180;	// Ротация " -> радиани
            double m = 1.00000377972;       // Мащабен коефициент
            double dx, dy, dz;

            dx = X - Xm;
            dy = Y - Ym;
            dz = Z - Zm;
            X = (dx + Rz * dy - Ry * dz) * m + dX + Xm;
            Y = (-Rz * dx + dy + Rx * dz) * m + dY + Ym;
            Z = (+Ry * dx - Rx * dy + dz) * m + dZ + Zm;
            return true;
        }

        bool XYZ4283_XYZ89(ref double X, ref double Y, ref double Z)
        {
            double Xm = 4188742.631;        // Централна точка
            double Ym = 2016041.319;
            double Zm = 4349746.951;
            double dX = +5.426;             // Транслация м
            double dY = -131.928;           // Транслация м
            double dZ = -103.833;           // Транслация м
            double Rx = -1.363718 * _PI / 648000.0;//=3600./180;	// Ротация " -> радиани
            double Ry = -1.980293 * _PI / 648000.0;//=3600./180;	// Ротация " -> радиани
            double Rz = +3.422231 * _PI / 648000.0;//=3600./180;	// Ротация " -> радиани
            double m = 1 / 1.00000377972;       // Мащабен коефициент
            double dx, dy, dz;

            dx = X - Xm;
            dy = Y - Ym;
            dz = Z - Zm;
            X = (dx + Rz * dy - Ry * dz) * m + dX + Xm;
            Y = (-Rz * dx + dy + Rx * dz) * m + dY + Ym;
            Z = (+Ry * dx - Rx * dy + dz) * m + dZ + Zm;

            return true;
        }


        // 
        bool XYZWGS84_XYZ4283(ref double X, ref double Y, ref double Z) // ??? Стандартен Красовски - WGS84
        {
            double dX = 5.8872e1;   // [m]
            double dY = 2.5781e1;   // [m]
            double dZ = 5.3637e1;   // [m]
            double f1 = -3.375288 * _PI / 3600 / 180;   // Ротация " -> радиани
            double f2 = 2.029736 * _PI / 3600 / 180;    // Ротация " -> радиани
            double f3 = 1.401645 * _PI / 3600 / 180;    // Ротация " -> радиани
            double m = 1 - 4.000687e-6;     // Мащаб
            double x = X;
            double y = Y;
            double z = Z;
            X = dX + m * x + f3 * y - f2 * z;
            Y = dY - f3 * x + m * y + f1 * z;
            Z = dZ + f2 * x - f1 * y + m * z;

            return true;
        }

        // Трансформация от и към координатни и височинни системи
        bool from1930_XYH1950(ushort coordZone, ushort coordType, ref double X50, ref double Y50, ref double Z50)
        {
            short currentZone = 0;
            double sFactor = 0.9999, reduct = 5e5;
            switch (coordType)                  // В зависимост от вида на координатите
            {
                case (ushort)CoordType.BLH:                           // Географски координати
                    Gauss_BL_XY((ushort)ellipsoid.HAYFORD, 3, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);    // -> В текуща зона проекционни координати
                    break;
                case (ushort)CoordType.XYH:                           // Проекционни координати в Гаусова проекция
                    if (coordZone == 6)
                    {
                        Gauss_XY_BL((ushort)ellipsoid.HAYFORD, coordZone, ref sFactor, ref reduct, ref X50, ref Y50);
                        Gauss_BL_XY((ushort)ellipsoid.HAYFORD, 3, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);
                    }
                    break;
                case (ushort)CoordType.XYZ:                           // Пространствени Декартови координати
                    XYZ_BLH((ushort)ellipsoid.HAYFORD, ref X50, ref Y50, ref Z50);    //
                    Gauss_BL_XY((ushort)ellipsoid.HAYFORD, 3, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);
                    break;
                default:
                    return false;
                    //break;
            }

            XY1930_XY1950_(1, ref X50, ref Y50);        // Трансформация от 1930 към 1950 за 3° ивица!
            sFactor = 1.0000;
            Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, 3, ref sFactor, ref reduct, ref X50, ref Y50);      // Гаус-Крюгерова проекция към Геодезически Географски координати
            Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50); // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!

            return true;
        }

        bool from1950_XYH1950(ushort coordZone, ushort coordType, ref double X50, ref double Y50, ref double Z50)
        {
            short currentZone = 0;
            double sFactor = 1.0, reduct = 5e5;
            switch (coordType)
            {
                case (ushort)CoordType.BLH:                   // Географски координати
                    Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);   // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!
                    break;
                case (ushort)CoordType.XYH:                   // Проекционни координати в Гаусова проекция
                    Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, coordZone, ref sFactor, ref reduct, ref X50, ref Y50);   // Гаус-Крюгерова проекция към Геодезически Географски координати
                    Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);      // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!
                    break;
                case (ushort)CoordType.XYZ:                   // Геоцентрични Декартови координати
                    XYZ_BLH((ushort)ellipsoid.KRASOVSKI, ref X50, ref Y50, ref Z50);
                    Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);      // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!
                    break;
                default:
                    return false;
            }

            return true;
        }

        bool fromSof_XYH1950(ushort coordZone, ushort coordType, ref double X50, ref double Y50)
        {
            short currentZone = 0;
            X50 += 4680000.000; // Транслация по ос X - север между КС1950 и Софийска
            Y50 += 8400000.000; // Транслация по ос Y - изток между КС9150 и Софийска
            double sFactor = 1.0, reduct = 5e5;
            Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, 3, ref sFactor, ref reduct, ref X50, ref Y50);       // Гаус-Крюгерова проекция 3° към Геодезически Географски координати
            Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);  // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!

            return true;
        }

        bool from1970_XYH1950(ushort coordZone, ref double X50, ref double Y50)
        {
            short currentZone = 0;
            XY1970_BL1950(coordZone, ref X50, ref Y50);     // Трансформация на проекционни координати от система “1970 г.” в географски координати в система “1950 г.” 
            double sFactor = 1.0, reduct = 5e5;
            Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X50, ref Y50);  // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!

            return true;
        }

        bool from1942_XYZ4283(ushort coordZone, ushort coordType, ref double X_1942, ref double Y_1942, ref double Z_1942)
        {
            short currentZone = 0;
            double sFactor = 1.0, reduct = 5e5;
            switch (coordType)
            {
                case (ushort)CoordType.BLH:                   // Географски координати
                    Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1942, ref Y_1942); // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!
                    break;
                case (ushort)CoordType.XYH:                   // Проекционни координати в Гаусова проекция
                    Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, coordZone, ref sFactor, ref reduct, ref X_1942, ref Y_1942); // Гаус-Крюгерова проекция към Геодезически Географски координати
                    Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1942, ref Y_1942);    // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!
                    break;
                case (ushort)CoordType.XYZ:                   // Геоцентрични Декартови координати
                    XYZ_BLH((ushort)ellipsoid.KRASOVSKI, ref X_1942, ref Y_1942, ref Z_1942);
                    Gauss_BL_XY((ushort)ellipsoid.KRASOVSKI, 6, ref currentZone, ref sFactor, ref reduct, ref X_1942, ref Y_1942);    // Геодезически Географски координати към Гаус-Крюгерова проекция в 6° ивица!
                    break;
                default:
                    return false;
            }
            XY1950_XY4283(1, ref X_1942, ref Y_1942);                           // от КС1942 към КС1942/83 6° ивица

            return true;
        }
        bool from2005_XYZ2005(ushort coordSys, ushort coordZone, ushort coordType, ref double X_2005, ref double Y_2005, ref double Z_2005)
        {
            switch (coordType)
            {
                case (ushort)CoordType.BLH:                               // Географски координати
                    BLH_XYZ((ushort)ellipsoid.GRS80, ref X_2005, ref Y_2005, ref Z_2005); // Трансформация към Декартови геоцентрични координати
                    break;
                case (ushort)CoordType.XYH:                               // Проекционни координати в Гаусова проекция, конична конформна Ламбертова проекция
                    if (coordZone > 0)                  // Проекционни координати е UTM проекция, зона 34 или 35
                    {
                        double sFactor = 0.9996, reduct = 5e5;

                        Gauss_XY_BL((ushort)ellipsoid.GRS80, coordZone, ref sFactor, ref reduct, ref X_2005, ref Y_2005); // UTM проекция към Геодезически Географски координати
                        BLH_XYZ((ushort)ellipsoid.GRS80, ref X_2005, ref Y_2005, ref Z_2005); // Трансформация към Декартови геоцентрични координати
                    }
                    else    // Кадастрална координатна система 2005 - конична конформна Ламбертова проекция, зона 0
                    {
                        double L0, B1, B2, Y0;
                        L0 = 25.0 + 30.0 / 60;             // Централен меридиан: 25°30'00.0000" в Градуси
                        B1 = 42.0;                       // Първи стандартен паралел: 42°00'00" в Градуси
                        B2 = 43.0 + 20.0 / 60;             // Втори стандартен паралел: 43°20'00" в Градуси
                        Y0 = 5e5;                       // Редукция на Y (изток): yyyyyy (500000)
                        Lambert2_XY_BL((ushort)ellipsoid.GRS80, ref L0, ref B1, ref B2, ref Y0, ref X_2005, ref Y_2005);      //  Конична конформна Ламбертова проекция с два стандартни паралела
                        BLH_XYZ((ushort)ellipsoid.GRS80, ref X_2005, ref Y_2005, ref Z_2005);                 // Трансформация към Декартови геоцентрични координати
                    }
                    break;
                case (ushort)CoordType.XYZ:                               // Геоцентрични Декартови координати

                    break;
                default:
                    return false;
            }
            return true;
        }

        bool from4283_XYZ4283(ushort fromCoordZone, ushort fromCoordType, ref double X_4283, ref double Y_4283, ref double Z_4283)
        {
            switch (fromCoordType)
            {
                case (ushort)CoordType.BLH:                   // Географски координати
                    BLH_XYZ((ushort)ellipsoid.KRASOVSKI, ref X_4283, ref Y_4283, ref Z_4283); // Трансформация към Декартови геоцентрични координати
                    break;
                case (ushort)CoordType.XYH:                   // Проекционни координати в Гаус-Крюгерова проекция
                    double sFactor = 1.0, reduct = 5e5;
                    Gauss_XY_BL((ushort)ellipsoid.KRASOVSKI, fromCoordZone, ref sFactor, ref reduct, ref X_4283, ref Y_4283); // Географски координати
                    BLH_XYZ((ushort)ellipsoid.KRASOVSKI, ref X_4283, ref Y_4283, ref Z_4283);                     // Трансформация към Декартови геоцентрични координати
                    break;
                case (ushort)CoordType.XYZ:                   // Геоцентрични Декартови координати
                    break;
                default:
                    return false;
            }

            return true;
        }

        bool fromBulRef89_XYZWGS84(ushort fromCoordZone, ushort fromCoordType, ref double X_WGS84, ref double Y_WGS84, ref double Z_WGS84)
        {
            switch (fromCoordType)
            {
                case (ushort)CoordType.BLH:                   // Географски координати
                    BLH_XYZ((ushort)ellipsoid.GRS80, ref X_WGS84, ref Y_WGS84, ref Z_WGS84);  // Трансформация към Декартови геоцентрични координати
                    break;
                case (ushort)CoordType.XYH:                   // Проекционни координати в UTM проекция
                    double sFactor = 0.9996, reduct = 5e5;
                    Gauss_XY_BL((ushort)ellipsoid.GRS80, fromCoordZone, ref sFactor, ref reduct, ref X_WGS84, ref Y_WGS84);   // Географски координати
                    BLH_XYZ((ushort)ellipsoid.GRS80, ref X_WGS84, ref Y_WGS84, ref Z_WGS84);                      // Трансформация към Декартови геоцентрични координати
                    break;
                case (ushort)CoordType.XYZ:                   // Геоцентрични Декартови координати
                    break;
                default:
                    return false;
            }

            return true;
        }


        bool to2005(ushort coordSys, ushort coordZone, ushort coordType, short currentZone,
            ref double x_2005, ref double y_2005, ref double z_2005)
        {
            switch (coordType)
            {
                case (ushort)CoordType.BLH:                   // Географски координати
                    XYZ_BLH((ushort)ellipsoid.GRS80, ref x_2005, ref y_2005, ref z_2005);
                    return true;
                case (ushort)CoordType.XYH:                   // Проекционни координати в Гаусова проекция
                    {
                        XYZ_BLH((ushort)ellipsoid.GRS80, ref x_2005, ref y_2005, ref z_2005);
                        if (coordZone > 0)                  // Проекционни координати е UTM проекция
                        {
                            double sFactor = 0.9996, reduct = 5e5;
                            Gauss_BL_XY((ushort)ellipsoid.GRS80, coordZone, ref currentZone, ref sFactor, ref reduct, ref x_2005, ref y_2005);    // Геодезически Географски към UTM проекция
                        }
                        else
                        {
                            double L0, B1, B2, Y0;
                            L0 = 25.0 + 30.0 / 60;             // Централен меридиан: 25°30'00.0000" в Градуси
                            B1 = 42.0;                       // Първи стандартен паралел: 42°00'00" в Градуси
                            B2 = 43.0 + 20.0 / 60;             // Втори стандартен паралел: 43°20'00" в Градуси
                            Y0 = 5e5;                       // Редукция на Y (изток): yyyyyy (500000)
                            Lambert2_BL_XY((ushort)ellipsoid.GRS80, ref L0, ref B1, ref B2, ref Y0, ref x_2005, ref y_2005);      //  Конична конформна Ламбертова проекция с два стандартни паралела
                        }
                    }
                    return true;
                case (ushort)CoordType.XYZ:                   // Геоцентрични Декартови координати

                    return true;
                default:
                    return false;
            }
        }

    }
}
