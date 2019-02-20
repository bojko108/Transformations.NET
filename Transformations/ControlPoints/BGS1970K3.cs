﻿using System.Collections.Generic;
using KDBush;

namespace BojkoSoft.Transformations.ControlPoints
{
    internal class BGS1970K3 : ControlPointsClass
    {
        public BGS1970K3() : base()
        {
            this.InitPoints();
            this.InitTree();
        }

        internal void InitPoints()
        {
            this.points = new List<Point<GeoPoint>>()
            {
                new GeoPoint(1, 4757148.066, 8599846.597, 0.0).ToPoint(),
                new GeoPoint(3, 4752644.234, 8594456.473, 0.0).ToPoint(),
                new GeoPoint(9381, 4731894.688, 8461052.569, 0.0).ToPoint(),
                new GeoPoint(8861, 4762130.192, 8523983.867, 0.0).ToPoint(),
                new GeoPoint(11466, 4717140.997, 8557525.041, 0.0).ToPoint(),
                new GeoPoint(8862, 4761723.423, 8533450.213, 0.0).ToPoint(),
                new GeoPoint(9383, 4730429.476, 8470299.495, 0.0).ToPoint(),
                new GeoPoint(6, 4749560.11, 8583823.293, 0.0).ToPoint(),
                new GeoPoint(7, 4749036.536, 8600597.699, 0.0).ToPoint(),
                new GeoPoint(10948, 4729738.711, 8530321.002, 0.0).ToPoint(),
                new GeoPoint(11472, 4712566.315, 8548964.541, 0.0).ToPoint(),
                new GeoPoint(8868, 4757095.337, 8537201.468, 0.0).ToPoint(),
                new GeoPoint(8870, 4756925.903, 8521439.908, 0.0).ToPoint(),
                new GeoPoint(8871, 4755216.378, 8530744.377, 0.0).ToPoint(),
                new GeoPoint(8879, 4749444.529, 8533327.267, 0.0).ToPoint(),
                new GeoPoint(8881, 4738856.429, 8440249.522, 0.0).ToPoint(),
                new GeoPoint(12010, 4708763.48, 8497331.465, 0.0).ToPoint(),
                new GeoPoint(12012, 4706478.567, 8492062.815, 0.0).ToPoint(),
                new GeoPoint(12536, 4688644.308, 8496784.826, 0.0).ToPoint(),
                new GeoPoint(7327, 4783256.972, 8451029.951, 0.0).ToPoint(),
                new GeoPoint(12017, 4704884.018, 8483219.606, 0.0).ToPoint(),
                new GeoPoint(12539, 4687550.492, 8490359.257, 0.0).ToPoint(),
                new GeoPoint(12540, 4686303.558, 8485823.686, 0.0).ToPoint(),
                new GeoPoint(7331, 4779643.079, 8456449.311, 0.0).ToPoint(),
                new GeoPoint(12020, 4701324.001, 8488861.845, 0.0).ToPoint(),
                new GeoPoint(12022, 4699703.513, 8497753.565, 0.0).ToPoint(),
                new GeoPoint(12544, 4683406.441, 8494266.266, 0.0).ToPoint(),
                new GeoPoint(7335, 4777144.024, 8447598.755, 0.0).ToPoint(),
                new GeoPoint(12025, 4698372.686, 8482967.543, 0.0).ToPoint(),
                new GeoPoint(12547, 4681436.551, 8498352.597, 0.0).ToPoint(),
                new GeoPoint(12548, 4679742.328, 8481786.846, 0.0).ToPoint(),
                new GeoPoint(12549, 4679518.363, 8489236.828, 0.0).ToPoint(),
                new GeoPoint(12029, 4695201.89, 8494417.5, 0.0).ToPoint(),
                new GeoPoint(7342, 4770392.673, 8456381.314, 0.0).ToPoint(),
                new GeoPoint(12031, 4694198.116, 8488487.925, 0.0).ToPoint(),
                new GeoPoint(7343, 4769626.787, 8445238.065, 0.0).ToPoint(),
                new GeoPoint(12032, 4693063.422, 8485062.392, 0.0).ToPoint(),
                new GeoPoint(13090, 4707949.963, 8555570.267, 0.0).ToPoint(),
                new GeoPoint(13092, 4707017.905, 8561262.689, 0.0).ToPoint(),
                new GeoPoint(3194, 4707652.197, 8568912.402, 0.0).ToPoint(),
                new GeoPoint(13094, 4706097.219, 8550252.322, 0.0).ToPoint(),
                new GeoPoint(3197, 4706679.75, 8578994.842, 0.0).ToPoint(),
                new GeoPoint(13096, 4705020.763, 8542006.645, 0.0).ToPoint(),
                new GeoPoint(3199, 4703442.519, 8564070.878, 0.0).ToPoint(),
                new GeoPoint(3200, 4703432.407, 8559024.334, 0.0).ToPoint(),
                new GeoPoint(5284, 4691557.849, 8651840.502, 0.0).ToPoint(),
                new GeoPoint(13099, 4703432.407, 8559024.334, 0.0).ToPoint(),
                new GeoPoint(3201, 4700849.245, 8567038.443, 0.0).ToPoint(),
                new GeoPoint(3202, 4700408.586, 8573586.376, 0.0).ToPoint(),
                new GeoPoint(13101, 4702557.663, 8552949.784, 0.0).ToPoint(),
                new GeoPoint(3203, 4700578.647, 8581742.088, 0.0).ToPoint(),
                new GeoPoint(13103, 4701218.397, 8547050.821, 0.0).ToPoint(),
                new GeoPoint(3205, 4698833.232, 8568518.168, 0.0).ToPoint(),
                new GeoPoint(3207, 4697218.06, 8562285.06, 0.0).ToPoint(),
                new GeoPoint(5293, 4681616.965, 8651839.576, 0.0).ToPoint(),
                new GeoPoint(6856, 4673229.425, 8653199.127, 0.0).ToPoint(),
                new GeoPoint(5294, 4681344.077, 8659102.299, 0.0).ToPoint(),
                new GeoPoint(6857, 4671916.856, 8660862.028, 0.0).ToPoint(),
                new GeoPoint(3211, 4693566.928, 8570746.896, 0.0).ToPoint(),
                new GeoPoint(6858, 4670883.39, 8647211.981, 0.0).ToPoint(),
                new GeoPoint(13110, 4697049.589, 8542678.324, 0.0).ToPoint(),
                new GeoPoint(3212, 4693477.873, 8582309.225, 0.0).ToPoint(),
                new GeoPoint(6339, 4654835.783, 8629673.119, 0.0).ToPoint(),
                new GeoPoint(6340, 4655250.641, 8642491.722, 0.0).ToPoint(),
                new GeoPoint(13113, 4695334.804, 8554957.976, 0.0).ToPoint(),
                new GeoPoint(13115, 4694431.003, 8550277.368, 0.0).ToPoint(),
                new GeoPoint(6343, 4652716.711, 8624112.658, 0.0).ToPoint(),
                new GeoPoint(6344, 4650225.407, 8633598.652, 0.0).ToPoint(),
                new GeoPoint(1657, 4745591.644, 8635357.241, 0.0).ToPoint(),
                new GeoPoint(6870, 4659848.197, 8657682.397, 0.0).ToPoint(),
                new GeoPoint(6871, 4658098.61, 8650091.343, 0.0).ToPoint(),
                new GeoPoint(6352, 4643869.671, 8626585.541, 0.0).ToPoint(),
                new GeoPoint(1664, 4740066.344, 8628551.757, 0.0).ToPoint(),
                new GeoPoint(6353, 4643815.075, 8637765.662, 0.0).ToPoint(),
                new GeoPoint(6354, 4643580.507, 8637615.321, 0.0).ToPoint(),
                new GeoPoint(1666, 4739605.654, 8638405.013, 0.0).ToPoint(),
                new GeoPoint(1669, 4736254.577, 8622636.065, 0.0).ToPoint(),
                new GeoPoint(1673, 4734374.825, 8631377.945, 0.0).ToPoint(),
                new GeoPoint(4801, 4692995.034, 8619190.99, 0.0).ToPoint(),
                new GeoPoint(4804, 4690114.153, 8612210.821, 0.0).ToPoint(),
                new GeoPoint(4811, 4685411.548, 8606011.901, 0.0).ToPoint(),
                new GeoPoint(4812, 4685368.746, 8618421.908, 0.0).ToPoint(),
                new GeoPoint(9504, 4725714.235, 8479253.288, 0.0).ToPoint(),
                new GeoPoint(4296, 4711042.323, 8626424.034, 0.0).ToPoint(),
                new GeoPoint(4297, 4710578.845, 8635659.125, 0.0).ToPoint(),
                new GeoPoint(9512, 4721509.519, 8463250.809, 0.0).ToPoint(),
                new GeoPoint(4824, 4677156.208, 8615007.457, 0.0).ToPoint(),
                new GeoPoint(9514, 4720152.204, 8469854.275, 0.0).ToPoint(),
                new GeoPoint(4304, 4705364.354, 8627188.933, 0.0).ToPoint(),
                new GeoPoint(4825, 4675398.31, 8604027.062, 0.0).ToPoint(),
                new GeoPoint(9517, 4718380.661, 8477153.78, 0.0).ToPoint(),
                new GeoPoint(7955, 4764994.427, 8452512.636, 0.0).ToPoint(),
                new GeoPoint(9519, 4716595.973, 8466814.3, 0.0).ToPoint(),
                new GeoPoint(7958, 4761779.85, 8459863.719, 0.0).ToPoint(),
                new GeoPoint(4312, 4700119.755, 8636799.887, 0.0).ToPoint(),
                new GeoPoint(7959, 4761687.203, 8446935.221, 0.0).ToPoint(),
                new GeoPoint(9523, 4713437.626, 8479399.743, 0.0).ToPoint(),
                new GeoPoint(9524, 4713290.576, 8473911.933, 0.0).ToPoint(),
                new GeoPoint(4314, 4699029.096, 8625382.296, 0.0).ToPoint(),
                new GeoPoint(9527, 4712000.876, 8464171.268, 0.0).ToPoint(),
                new GeoPoint(7964, 4757640.814, 8453345.976, 0.0).ToPoint(),
                new GeoPoint(4319, 4695336.759, 8633125.146, 0.0).ToPoint(),
                new GeoPoint(154, 4749163.633, 8565165.669, 0.0).ToPoint(),
                new GeoPoint(155, 4748780.129, 8573487.633, 0.0).ToPoint(),
                new GeoPoint(5886, 4669185.127, 8617333.77, 0.0).ToPoint(),
                new GeoPoint(7971, 4755004.238, 8447211.935, 0.0).ToPoint(),
                new GeoPoint(7975, 4751778.098, 8455471.664, 0.0).ToPoint(),
                new GeoPoint(3807, 4691518.634, 8566149.576, 0.0).ToPoint(),
                new GeoPoint(5891, 4664602.468, 8610594.579, 0.0).ToPoint(),
                new GeoPoint(3811, 4687684.177, 8576197.353, 0.0).ToPoint(),
                new GeoPoint(3812, 4686144.001, 8566965.034, 0.0).ToPoint(),
                new GeoPoint(5896, 4660116.761, 8618342.334, 0.0).ToPoint(),
                new GeoPoint(5899, 4658389.451, 8609285.115, 0.0).ToPoint(),
                new GeoPoint(691, 4748185.941, 8594346.563, 0.0).ToPoint(),
                new GeoPoint(3817, 4682738.322, 8573068.55, 0.0).ToPoint(),
                new GeoPoint(3818, 4681767.35, 8582430.347, 0.0).ToPoint(),
                new GeoPoint(693, 4747112.484, 8600185.378, 0.0).ToPoint(),
                new GeoPoint(8509, 4761488.433, 8491629.406, 0.0).ToPoint(),
                new GeoPoint(3820, 4679254.843, 8576511.712, 0.0).ToPoint(),
                new GeoPoint(3822, 4677898.73, 8566851.308, 0.0).ToPoint(),
                new GeoPoint(7992, 4766909.772, 8513272.617, 0.0).ToPoint(),
                new GeoPoint(8514, 4757921.837, 8481585.598, 0.0).ToPoint(),
                new GeoPoint(699, 4741255.082, 8585069.963, 0.0).ToPoint(),
                new GeoPoint(8515, 4757300.513, 8499155.102, 0.0).ToPoint(),
                new GeoPoint(700, 4741457.929, 8594391.311, 0.0).ToPoint(),
                new GeoPoint(702, 4739857.555, 8599572.615, 0.0).ToPoint(),
                new GeoPoint(1226, 4728715.486, 8569546.232, 0.0).ToPoint(),
                new GeoPoint(8524, 4751168.23, 8491013.279, 0.0).ToPoint(),
                new GeoPoint(709, 4733684.15, 8589043.835, 0.0).ToPoint(),
                new GeoPoint(10089, 4740173.708, 8515667.812, 0.0).ToPoint(),
                new GeoPoint(711, 4731572.279, 8595031.571, 0.0).ToPoint(),
                new GeoPoint(1232, 4723268.246, 8578999.591, 0.0).ToPoint(),
                new GeoPoint(10090, 4740174.039, 8506488.07, 0.0).ToPoint(),
                new GeoPoint(9572, 4729331.238, 8451687.985, 0.0).ToPoint(),
                new GeoPoint(10093, 4737336.315, 8501287.955, 0.0).ToPoint(),
                new GeoPoint(1236, 4721095.367, 8571285.63, 0.0).ToPoint(),
                new GeoPoint(1238, 4718468.91, 8563944.189, 0.0).ToPoint(),
                new GeoPoint(9575, 4727461.769, 8457204.033, 0.0).ToPoint(),
                new GeoPoint(6972, 4650697.197, 8657522.905, 0.0).ToPoint(),
                new GeoPoint(2804, 4730476.907, 8650873.156, 0.0).ToPoint(),
                new GeoPoint(6973, 4649847.556, 8645929.043, 0.0).ToPoint(),
                new GeoPoint(9578, 4725484.002, 8446560.077, 0.0).ToPoint(),
                new GeoPoint(2805, 4728945.732, 8643488.638, 0.0).ToPoint(),
                new GeoPoint(1243, 4713477.239, 8563647.73, 0.0).ToPoint(),
                new GeoPoint(10101, 4731571.508, 8513569.761, 0.0).ToPoint(),
                new GeoPoint(1244, 4713425.976, 8572061.456, 0.0).ToPoint(),
                new GeoPoint(9582, 4722108.745, 8451622.048, 0.0).ToPoint(),
                new GeoPoint(1246, 4712682.88, 8578440.149, 0.0).ToPoint(),
                new GeoPoint(6978, 4644120.631, 8652016.527, 0.0).ToPoint(),
                new GeoPoint(10104, 4730253.374, 8505540.244, 0.0).ToPoint(),
                new GeoPoint(9585, 4717772.172, 8454757.6, 0.0).ToPoint(),
                new GeoPoint(2812, 4725698.917, 8661202.167, 0.0).ToPoint(),
                new GeoPoint(2813, 4723847.545, 8648859.687, 0.0).ToPoint(),
                new GeoPoint(2814, 4724093.265, 8655350.322, 0.0).ToPoint(),
                new GeoPoint(211, 4753678.186, 8635743.716, 0.0).ToPoint(),
                new GeoPoint(13239, 4690694.066, 8561340.734, 0.0).ToPoint(),
                new GeoPoint(215, 4750133.151, 8624183.492, 0.0).ToPoint(),
                new GeoPoint(13240, 4690901.042, 8543386.638, 0.0).ToPoint(),
                new GeoPoint(216, 4750395.83, 8631373.234, 0.0).ToPoint(),
                new GeoPoint(2822, 4718275.071, 8644857.631, 0.0).ToPoint(),
                new GeoPoint(13243, 4687697.139, 8552642.782, 0.0).ToPoint(),
                new GeoPoint(11163, 4745231.98, 8551525.364, 0.0).ToPoint(),
                new GeoPoint(2828, 4714057.865, 8652601.849, 0.0).ToPoint(),
                new GeoPoint(11164, 4744825.715, 8557726.049, 0.0).ToPoint(),
                new GeoPoint(13248, 4684615.988, 8558773.479, 0.0).ToPoint(),
                new GeoPoint(13249, 4684798.43, 8544286.391, 0.0).ToPoint(),
                new GeoPoint(11166, 4742465.998, 8542615.603, 0.0).ToPoint(),
                new GeoPoint(11689, 4728200.88, 8537668.749, 0.0).ToPoint(),
                new GeoPoint(13256, 4679519.379, 8559262.361, 0.0).ToPoint(),
                new GeoPoint(11173, 4736964.971, 8552165.378, 0.0).ToPoint(),
                new GeoPoint(13257, 4679481.116, 8550615.806, 0.0).ToPoint(),
                new GeoPoint(13258, 4677508.108, 8545154.795, 0.0).ToPoint(),
                new GeoPoint(11175, 4736438.756, 8541828.164, 0.0).ToPoint(),
                new GeoPoint(11696, 4723245.447, 8525612.023, 0.0).ToPoint(),
                new GeoPoint(11177, 4733911.507, 8561272.495, 0.0).ToPoint(),
                new GeoPoint(11698, 4722058.588, 8537895.475, 0.0).ToPoint(),
                new GeoPoint(5447, 4669394.971, 8569967.463, 0.0).ToPoint(),
                new GeoPoint(11181, 4731842.867, 8552412.58, 0.0).ToPoint(),
                new GeoPoint(11703, 4718368.783, 8531143.578, 0.0).ToPoint(),
                new GeoPoint(10665, 4729042.318, 8499546.428, 0.0).ToPoint(),
                new GeoPoint(10666, 4727602.246, 8492674.682, 0.0).ToPoint(),
                new GeoPoint(11708, 4714457.315, 8541014.785, 0.0).ToPoint(),
                new GeoPoint(3375, 4707002.37, 8598305.157, 0.0).ToPoint(),
                new GeoPoint(3376, 4706130.855, 8583621.7, 0.0).ToPoint(),
                new GeoPoint(11712, 4711910.593, 8522542.016, 0.0).ToPoint(),
                new GeoPoint(3377, 4705674.185, 8591688.295, 0.0).ToPoint(),
                new GeoPoint(10671, 4724092.615, 8486998.442, 0.0).ToPoint(),
                new GeoPoint(11714, 4710979.144, 8529371.985, 0.0).ToPoint(),
                new GeoPoint(255, 4757140.561, 8608055.735, 0.0).ToPoint(),
                new GeoPoint(10676, 4720658.848, 8492651.282, 0.0).ToPoint(),
                new GeoPoint(13802, 4803204.303, 8461022.274, 0.0).ToPoint(),
                new GeoPoint(259, 4753701.516, 8615000.978, 0.0).ToPoint(),
                new GeoPoint(13805, 4797370.023, 8467936.443, 0.0).ToPoint(),
                new GeoPoint(260, 4752833.24, 8602847.406, 0.0).ToPoint(),
                new GeoPoint(3386, 4698034.063, 8590285.856, 0.0).ToPoint(),
                new GeoPoint(262, 4750384.219, 8606295.557, 0.0).ToPoint(),
                new GeoPoint(3388, 4697356.207, 8600459.803, 0.0).ToPoint(),
                new GeoPoint(10682, 4717333.234, 8481903.477, 0.0).ToPoint(),
                new GeoPoint(13809, 4795254.867, 8477375.851, 0.0).ToPoint(),
                new GeoPoint(12247, 4709295.342, 8516603.121, 0.0).ToPoint(),
                new GeoPoint(3391, 4693924.491, 8590460.157, 0.0).ToPoint(),
                new GeoPoint(13811, 4794625.173, 8461926.046, 0.0).ToPoint(),
                new GeoPoint(10687, 4713586.407, 8492274.071, 0.0).ToPoint(),
                new GeoPoint(12250, 4707737.75, 8508807.379, 0.0).ToPoint(),
                new GeoPoint(10688, 4712930.916, 8500496.334, 0.0).ToPoint(),
                new GeoPoint(12251, 4707523.73, 8504242.462, 0.0).ToPoint(),
                new GeoPoint(7042, 4779098.17, 8433465.731, 0.0).ToPoint(),
                new GeoPoint(7043, 4777629.67, 8440279.696, 0.0).ToPoint(),
                new GeoPoint(10692, 4711170.872, 8484218.712, 0.0).ToPoint(),
                new GeoPoint(13818, 4789342.67, 8472013.815, 0.0).ToPoint(),
                new GeoPoint(12256, 4702940.139, 8501447.482, 0.0).ToPoint(),
                new GeoPoint(12257, 4702221.893, 8510665.943, 0.0).ToPoint(),
                new GeoPoint(13820, 4788090.386, 8466194.838, 0.0).ToPoint(),
                new GeoPoint(12258, 4701560.279, 8516016.528, 0.0).ToPoint(),
                new GeoPoint(13821, 4786844.332, 8477080.237, 0.0).ToPoint(),
                new GeoPoint(7049, 4769287.194, 8437022.14, 0.0).ToPoint(),
                new GeoPoint(6529, 4648660.389, 8615057.843, 0.0).ToPoint(),
                new GeoPoint(9135, 4747804.819, 8451275.274, 0.0).ToPoint(),
                new GeoPoint(12262, 4698269.969, 8503562.178, 0.0).ToPoint(),
                new GeoPoint(7575, 4784014.57, 8471639.833, 0.0).ToPoint(),
                new GeoPoint(9138, 4746641.232, 8441091.648, 0.0).ToPoint(),
                new GeoPoint(7576, 4783174.273, 8461055.353, 0.0).ToPoint(),
                new GeoPoint(9139, 4743630.457, 8457316.753, 0.0).ToPoint(),
                new GeoPoint(7577, 4782408.862, 8465042.246, 0.0).ToPoint(),
                new GeoPoint(12266, 4694837.588, 8511636.778, 0.0).ToPoint(),
                new GeoPoint(12267, 4693778.01, 8501512.115, 0.0).ToPoint(),
                new GeoPoint(9143, 4742301.363, 8446948.631, 0.0).ToPoint(),
                new GeoPoint(12270, 4692999.737, 8507338.734, 0.0).ToPoint(),
                new GeoPoint(2373, 4728996.471, 8608958.502, 0.0).ToPoint(),
                new GeoPoint(6541, 4637827.598, 8607525.136, 0.0).ToPoint(),
                new GeoPoint(7583, 4775440.944, 8460889.397, 0.0).ToPoint(),
                new GeoPoint(7588, 4768541.219, 8463501.664, 0.0).ToPoint(),
                new GeoPoint(9151, 4736382.495, 8452649.376, 0.0).ToPoint(),
                new GeoPoint(7589, 4766562.514, 8468612.867, 0.0).ToPoint(),
                new GeoPoint(2384, 4722123.95, 8617393.948, 0.0).ToPoint(),
                new GeoPoint(2385, 4719944.711, 8607193.668, 0.0).ToPoint(),
                new GeoPoint(9160, 4731295.936, 8444368.153, 0.0).ToPoint(),
                new GeoPoint(11766, 4708024.61, 8473727.437, 0.0).ToPoint(),
                new GeoPoint(11767, 4707605.818, 8467732.857, 0.0).ToPoint(),
                new GeoPoint(11769, 4705661.714, 8479457.924, 0.0).ToPoint(),
                new GeoPoint(2392, 4713937.998, 8603500.385, 0.0).ToPoint(),
                new GeoPoint(2394, 4713574.541, 8612257.324, 0.0).ToPoint(),
                new GeoPoint(11772, 4703495.672, 8470211.18, 0.0).ToPoint(),
                new GeoPoint(11777, 4697408.942, 8474975.898, 0.0).ToPoint(),
                new GeoPoint(7118, 4766384.641, 8430428.536, 0.0).ToPoint(),
                new GeoPoint(5036, 4710556.596, 8644578.918, 0.0).ToPoint(),
                new GeoPoint(7123, 4759661.828, 8439068.063, 0.0).ToPoint(),
                new GeoPoint(5040, 4707493.83, 8651742.806, 0.0).ToPoint(),
                new GeoPoint(1916, 4727358.448, 8623605.974, 0.0).ToPoint(),
                new GeoPoint(5042, 4706724.216, 8660493.292, 0.0).ToPoint(),
                new GeoPoint(7126, 4753697.316, 8432987.219, 0.0).ToPoint(),
                new GeoPoint(7127, 4753257.471, 8438165.699, 0.0).ToPoint(),
                new GeoPoint(1919, 4726046.662, 8629512.036, 0.0).ToPoint(),
                new GeoPoint(5047, 4700328.213, 8645454.558, 0.0).ToPoint(),
                new GeoPoint(5050, 4697793.525, 8656248.552, 0.0).ToPoint(),
                new GeoPoint(1925, 4722598.068, 8636443.929, 0.0).ToPoint(),
                new GeoPoint(1930, 4718193.036, 8623213.36, 0.0).ToPoint(),
                new GeoPoint(1934, 4716126.459, 8640210.188, 0.0).ToPoint(),
                new GeoPoint(1935, 4715336.333, 8630243.468, 0.0).ToPoint(),
                new GeoPoint(11835, 4687212.919, 8479617.949, 0.0).ToPoint(),
                new GeoPoint(4543, 4691055.391, 8639054.569, 0.0).ToPoint(),
                new GeoPoint(11837, 4682664.764, 8474161.825, 0.0).ToPoint(),
                new GeoPoint(4546, 4689726.175, 8626849.116, 0.0).ToPoint(),
                new GeoPoint(8714, 4754756.819, 8558782.398, 0.0).ToPoint(),
                new GeoPoint(8715, 4754933.285, 8545497.051, 0.0).ToPoint(),
                new GeoPoint(8717, 4752116.981, 8551885.013, 0.0).ToPoint(),
                new GeoPoint(8721, 4749328.865, 8543593.998, 0.0).ToPoint(),
                new GeoPoint(5597, 4672508.59, 8589094.297, 0.0).ToPoint(),
                new GeoPoint(4556, 4683310.797, 8634923.494, 0.0).ToPoint(),
                new GeoPoint(5600, 4669220.161, 8599569.825, 0.0).ToPoint(),
                new GeoPoint(4560, 4680870.325, 8642906.217, 0.0).ToPoint(),
                new GeoPoint(5602, 4668236.782, 8582772.686, 0.0).ToPoint(),
                new GeoPoint(4040, 4711071.794, 8619242.711, 0.0).ToPoint(),
                new GeoPoint(4562, 4678727.983, 8626919.775, 0.0).ToPoint(),
                new GeoPoint(5604, 4666468.286, 8591128.952, 0.0).ToPoint(),
                new GeoPoint(4045, 4708011.081, 8604790.67, 0.0).ToPoint(),
                new GeoPoint(4566, 4677037.77, 8637877.01, 0.0).ToPoint(),
                new GeoPoint(4053, 4701338.827, 8616285.296, 0.0).ToPoint(),
                new GeoPoint(929, 4727447.821, 8582686.02, 0.0).ToPoint(),
                new GeoPoint(932, 4724809.134, 8597517.255, 0.0).ToPoint(),
                new GeoPoint(4058, 4697155.755, 8608396.451, 0.0).ToPoint(),
                new GeoPoint(934, 4723097.21, 8588035.095, 0.0).ToPoint(),
                new GeoPoint(938, 4719505.359, 8595952.286, 0.0).ToPoint(),
                new GeoPoint(13965, 4801761.766, 8453932.903, 0.0).ToPoint(),
                new GeoPoint(1463, 4748107.476, 8613320.637, 0.0).ToPoint(),
                new GeoPoint(943, 4715047.087, 8589329.782, 0.0).ToPoint(),
                new GeoPoint(1465, 4745077.442, 8608110.006, 0.0).ToPoint(),
                new GeoPoint(10325, 4724723.382, 8519080.984, 0.0).ToPoint(),
                new GeoPoint(13972, 4793324.566, 8450659.754, 0.0).ToPoint(),
                new GeoPoint(1469, 4743232.008, 8621013.204, 0.0).ToPoint(),
                new GeoPoint(13973, 4791521.097, 8456853.515, 0.0).ToPoint(),
                new GeoPoint(1473, 4739067.437, 8610293.85, 0.0).ToPoint(),
                new GeoPoint(8247, 4763391.932, 8502716.906, 0.0).ToPoint(),
                new GeoPoint(10331, 4721179.288, 8505513.265, 0.0).ToPoint(),
                new GeoPoint(13979, 4787791.591, 8443564.368, 0.0).ToPoint(),
                new GeoPoint(8249, 4761486.045, 8510823.896, 0.0).ToPoint(),
                new GeoPoint(10336, 4717079.52, 8520228.756, 0.0).ToPoint(),
                new GeoPoint(12941, 4706740.949, 8532185.12, 0.0).ToPoint(),
                new GeoPoint(12943, 4705935.42, 8537065.731, 0.0).ToPoint(),
                new GeoPoint(8255, 4757771.202, 8505808.646, 0.0).ToPoint(),
                new GeoPoint(10339, 4713897.237, 8511528.487, 0.0).ToPoint(),
                new GeoPoint(1482, 4733843.343, 8615518.716, 0.0).ToPoint(),
                new GeoPoint(12944, 4705875.191, 8521772.568, 0.0).ToPoint(),
                new GeoPoint(13465, 4690966.93, 8537171.556, 0.0).ToPoint(),
                new GeoPoint(12945, 4703879.326, 8526932.684, 0.0).ToPoint(),
                new GeoPoint(13467, 4689247.096, 8527415.539, 0.0).ToPoint(),
                new GeoPoint(9821, 4746381.958, 8498647.679, 0.0).ToPoint(),
                new GeoPoint(13468, 4689071.417, 8522692.803, 0.0).ToPoint(),
                new GeoPoint(6176, 4669156.758, 8641802.208, 0.0).ToPoint(),
                new GeoPoint(8260, 4754054.249, 8510506.534, 0.0).ToPoint(),
                new GeoPoint(12949, 4701074.644, 8531426.79, 0.0).ToPoint(),
                new GeoPoint(13470, 4687596.41, 8532498.622, 0.0).ToPoint(),
                new GeoPoint(6178, 4668205.161, 8634484.029, 0.0).ToPoint(),
                new GeoPoint(8262, 4752328.641, 8520160.439, 0.0).ToPoint(),
                new GeoPoint(9825, 4743492.011, 8486418.171, 0.0).ToPoint(),
                new GeoPoint(13472, 4686469.326, 8539178.378, 0.0).ToPoint(),
                new GeoPoint(9826, 4741976.965, 8493057.272, 0.0).ToPoint(),
                new GeoPoint(12952, 4699704.505, 8537244.365, 0.0).ToPoint(),
                new GeoPoint(6180, 4665581.44, 8624180.865, 0.0).ToPoint(),
                new GeoPoint(8264, 4750923.65, 8504186.77, 0.0).ToPoint(),
                new GeoPoint(12953, 4699731.599, 8524186.938, 0.0).ToPoint(),
                new GeoPoint(13474, 4685818.894, 8522857.403, 0.0).ToPoint(),
                new GeoPoint(8266, 4748498.188, 8518206.679, 0.0).ToPoint(),
                new GeoPoint(12434, 4690043.585, 8513038.698, 0.0).ToPoint(),
                new GeoPoint(8267, 4748703.051, 8509180.333, 0.0).ToPoint(),
                new GeoPoint(3578, 4691224.711, 8600653.172, 0.0).ToPoint(),
                new GeoPoint(12435, 4688503.893, 8519312.088, 0.0).ToPoint(),
                new GeoPoint(13477, 4683325.466, 8528637.947, 0.0).ToPoint(),
                new GeoPoint(6184, 4663198.667, 8631541.588, 0.0).ToPoint(),
                new GeoPoint(12436, 4687373.488, 8504948.643, 0.0).ToPoint(),
                new GeoPoint(12957, 4695862.335, 8521976.351, 0.0).ToPoint(),
                new GeoPoint(3580, 4688545.544, 8591801.766, 0.0).ToPoint(),
                new GeoPoint(12958, 4695424.768, 8532594.385, 0.0).ToPoint(),
                new GeoPoint(13479, 4681300.107, 8539714.498, 0.0).ToPoint(),
                new GeoPoint(6186, 4661642.467, 8638144.717, 0.0).ToPoint(),
                new GeoPoint(13481, 4680111.791, 8533168.447, 0.0).ToPoint(),
                new GeoPoint(12961, 4693136.552, 8526707.328, 0.0).ToPoint(),
                new GeoPoint(7754, 4761963.188, 8475289.033, 0.0).ToPoint(),
                new GeoPoint(9838, 4735604.607, 8487335.181, 0.0).ToPoint(),
                new GeoPoint(12443, 4683967.379, 8501407.177, 0.0).ToPoint(),
                new GeoPoint(13485, 4678391.859, 8522955.398, 0.0).ToPoint(),
                new GeoPoint(12444, 4682423.616, 8517011.132, 0.0).ToPoint(),
                new GeoPoint(9841, 4733705.961, 8496790.699, 0.0).ToPoint(),
                new GeoPoint(3589, 4682242.061, 8590036.748, 0.0).ToPoint(),
                new GeoPoint(7758, 4759706.565, 8468825.727, 0.0).ToPoint(),
                new GeoPoint(12447, 4680702.865, 8509222.939, 0.0).ToPoint(),
                new GeoPoint(3592, 4679551.921, 8597478.921, 0.0).ToPoint(),
                new GeoPoint(12453, 4677701.993, 8501824.627, 0.0).ToPoint(),
                new GeoPoint(7765, 4756003.993, 8461727.385, 0.0).ToPoint(),
                new GeoPoint(7768, 4754104.742, 8473038.01, 0.0).ToPoint(),
                new GeoPoint(475, 4744292.354, 8577615.591, 0.0).ToPoint(),
                new GeoPoint(477, 4742197.186, 8571228.076, 0.0).ToPoint(),
                new GeoPoint(7773, 4752436.811, 8467046.284, 0.0).ToPoint(),
                new GeoPoint(479, 4741040.694, 8562484.554, 0.0).ToPoint(),
                new GeoPoint(7778, 4748837.902, 8480327.721, 0.0).ToPoint(),
                new GeoPoint(484, 4736925.812, 8562958.145, 0.0).ToPoint(),
                new GeoPoint(7779, 4748222.015, 8462787.314, 0.0).ToPoint(),
                new GeoPoint(485, 4737456.108, 8576143.883, 0.0).ToPoint(),
                new GeoPoint(2570, 4747687.021, 8642225.85, 0.0).ToPoint(),
                new GeoPoint(2571, 4747694.213, 8652552.086, 0.0).ToPoint(),
                new GeoPoint(487, 4735649.535, 8570037.671, 0.0).ToPoint(),
                new GeoPoint(493, 4730617.07, 8562947.412, 0.0).ToPoint(),
                new GeoPoint(2581, 4739768.529, 8651031.33, 0.0).ToPoint(),
                new GeoPoint(2582, 4739164.092, 8657609.523, 0.0).ToPoint(),
                new GeoPoint(2584, 4737941.211, 8643929.544, 0.0).ToPoint(),
                new GeoPoint(9357, 4747615.822, 8468043.724, 0.0).ToPoint(),
                new GeoPoint(9360, 4745561.263, 8475507.954, 0.0).ToPoint(),
                new GeoPoint(10923, 4746508.339, 8524922.714, 0.0).ToPoint(),
                new GeoPoint(10927, 4743304.609, 8528563.776, 0.0).ToPoint(),
                new GeoPoint(9367, 4740132.183, 8475302.538, 0.0).ToPoint(),
                new GeoPoint(2595, 4731497.367, 8657032.657, 0.0).ToPoint(),
                new GeoPoint(11452, 4728287.506, 8544853.048, 0.0).ToPoint(),
                new GeoPoint(10932, 4739962.906, 8537332.307, 0.0).ToPoint(),
                new GeoPoint(11453, 4727296.123, 8558223.685, 0.0).ToPoint(),
                new GeoPoint(9371, 4737814.754, 8468846.127, 0.0).ToPoint(),
                new GeoPoint(11458, 4723040.855, 8547885.136, 0.0).ToPoint(),
                new GeoPoint(10939, 4737085.992, 8522210.444, 0.0).ToPoint(),
                new GeoPoint(9377, 4734000.094, 8480327.89, 0.0).ToPoint(),
                new GeoPoint(10940, 4736017.392, 8533124.895, 0.0).ToPoint()
            };
        }
    }
}