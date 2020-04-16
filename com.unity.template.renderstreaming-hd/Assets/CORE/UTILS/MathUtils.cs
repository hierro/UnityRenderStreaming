using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

using Sirenix.OdinInspector;

namespace CORE.UTILS
{

    public static class MathUtils
    {

        #region CONSTANTS

        const float pi = Mathf.PI;

        const float _PiOver2 = Mathf.PI * 0.5f;
        const float _TwoPi = Mathf.PI * 2;

        #endregion

        #region MISC


        public static int[] RandomIndexedArray(int lenght)
        {

            int i = 0;
            int[] array = new int[lenght];

            while (i < array.Length)
            {
                array[i] = i;
                i++;
            }

            System.Random rnd = new System.Random();
            return array.OrderBy(x => rnd.Next()).ToArray();
        }

        public static int RandomSign()
        {
            return UnityEngine.Random.value < 0.5f ? 1 : -1;
        }

        /*
        public static bool IsPositive(this float me )
        {
            return me > 0;
        }
        */


        public static float ValueInRange(Vector2 range, bool randomSign = false)
        {

            float result = UnityEngine.Random.Range(range.x, range.y);

            if (randomSign)
            {
                result = result * RandomSign();
            }

            return result;
        }

        #endregion

        #region EASE
        public enum Ease
        {
            Unset, // Used to let TweenParams know that the ease was not set and apply it differently if used on Tweeners or Sequences
            Linear,
            InSine,
            OutSine,
            InOutSine,
            InQuad,
            OutQuad,
            InOutQuad,
            InCubic,
            OutCubic,
            InOutCubic,
            InQuart,
            OutQuart,
            InOutQuart,
            InQuint,
            OutQuint,
            InOutQuint,
            InExpo,
            OutExpo,
            InOutExpo,
            InCirc,
            OutCirc,
            InOutCirc,
            InElastic,
            OutElastic,
            InOutElastic,
            InBack,
            OutBack,
            InOutBack,
            InBounce,
            OutBounce,
            InOutBounce,
            /// <summary>
            /// Don't assign this! It's assigned automatically when creating 0 duration tweens
            /// </summary>
            INTERNAL_Zero,
            /// <summary>
            /// Don't assign this! It's assigned automatically when setting the ease to an AnimationCurve or to a custom ease function
            /// </summary>
            INTERNAL_Custom
        }
        public static float CalculateByEase(float time, float duration, Ease easeType, float overshootOrAmplitude, float period)
        {
            switch (easeType)
            {
                case Ease.Linear:
                    return time / duration;
                case Ease.InSine:
                    return -(float)Math.Cos(time / duration * _PiOver2) + 1;
                case Ease.OutSine:
                    return (float)Math.Sin(time / duration * _PiOver2);
                case Ease.InOutSine:
                    return -0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1);
                case Ease.InQuad:
                    return (time /= duration) * time;
                case Ease.OutQuad:
                    return -(time /= duration) * (time - 2);
                case Ease.InOutQuad:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time;
                    return -0.5f * ((--time) * (time - 2) - 1);
                case Ease.InCubic:
                    return (time /= duration) * time * time;
                case Ease.OutCubic:
                    return ((time = time / duration - 1) * time * time + 1);
                case Ease.InOutCubic:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time;
                    return 0.5f * ((time -= 2) * time * time + 2);
                case Ease.InQuart:
                    return (time /= duration) * time * time * time;
                case Ease.OutQuart:
                    return -((time = time / duration - 1) * time * time * time - 1);
                case Ease.InOutQuart:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time;
                    return -0.5f * ((time -= 2) * time * time * time - 2);
                case Ease.InQuint:
                    return (time /= duration) * time * time * time * time;
                case Ease.OutQuint:
                    return ((time = time / duration - 1) * time * time * time * time + 1);
                case Ease.InOutQuint:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time * time;
                    return 0.5f * ((time -= 2) * time * time * time * time + 2);
                case Ease.InExpo:
                    return (time == 0) ? 0 : (float)Math.Pow(2, 10 * (time / duration - 1));
                case Ease.OutExpo:
                    if (time == duration) return 1;
                    return (-(float)Math.Pow(2, -10 * time / duration) + 1);
                case Ease.InOutExpo:
                    if (time == 0) return 0;
                    if (time == duration) return 1;
                    if ((time /= duration * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (time - 1));
                    return 0.5f * (-(float)Math.Pow(2, -10 * --time) + 2);
                case Ease.InCirc:
                    return -((float)Math.Sqrt(1 - (time /= duration) * time) - 1);
                case Ease.OutCirc:
                    return (float)Math.Sqrt(1 - (time = time / duration - 1) * time);
                case Ease.InOutCirc:
                    if ((time /= duration * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - time * time) - 1);
                    return 0.5f * ((float)Math.Sqrt(1 - (time -= 2) * time) + 1);
                case Ease.InElastic:
                    float s0;
                    if (time == 0) return 0;
                    if ((time /= duration) == 1) return 1;
                    if (period == 0) period = duration * 0.3f;
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s0 = period / 4;
                    }
                    else s0 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * _TwoPi / period));
                case Ease.OutElastic:
                    float s1;
                    if (time == 0) return 0;
                    if ((time /= duration) == 1) return 1;
                    if (period == 0) period = duration * 0.3f;
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s1 = period / 4;
                    }
                    else s1 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * _TwoPi / period) + 1);
                case Ease.InOutElastic:
                    float s;
                    if (time == 0) return 0;
                    if ((time /= duration * 0.5f) == 2) return 1;
                    if (period == 0) period = duration * (0.3f * 1.5f);
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s = period / 4;
                    }
                    else s = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    if (time < 1) return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period));
                    return overshootOrAmplitude * (float)Math.Pow(2, -10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period) * 0.5f + 1;
                case Ease.InBack:
                    return (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude);
                case Ease.OutBack:
                    return ((time = time / duration - 1) * time * ((overshootOrAmplitude + 1) * time + overshootOrAmplitude) + 1);
                case Ease.InOutBack:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude));
                    return 0.5f * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2);
                /*
            case Ease.InBounce:
                return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
            case Ease.OutBounce:
                return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
            case Ease.InOutBounce:
                return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
                return t.customEase(time, duration, overshootOrAmplitude, period);
                 */
                case Ease.INTERNAL_Zero:
                    // 0 duration tween
                    return 1;
                default:
                    // OutQuad
                    return -(time /= duration) * (time - 2);
            }
        }
        #endregion

        #region SEQUENCING

        public static float[] GenerateEasedSequence(int lenght, float baseValue = 1, Ease easeType = Ease.Linear)
        {
            float[] values = new float[lenght + 1];

            for (int a = 0; a < lenght + 1; a++)
            {
                values[a] = CalculateByEase((float)(a + 1), (float)(lenght + 1), easeType, 0f, 0f) * baseValue;
            }

            return values;
        }

        public static List<float> GenerateEasedList(int lenght, float baseValue = 1, Ease easeType = Ease.Linear)
        {
            return GenerateEasedSequence(lenght, baseValue, easeType).ToList();
        }

        public static List<int> GenerateSqauresSequence(int min = 1, int max = 10)
        {
            List<int> squares = Enumerable.Range(min, max).Select(x => x * x).ToList();

            return squares;

        }
        public static float[] GenerateLogSpace(int min, int max, int logBins)
        {
            double logarithmicBase = Math.E;
            double logMin = Math.Log10(min);
            double logMax = Math.Log10(max);
            double delta = (logMax - logMin) / logBins;
            int[] indexes = new int[logBins + 1];
            double accDelta = 0;
            float[] v = new float[logBins];
            for (int i = 0; i <= logBins; ++i)
            {
                v[i] = (float)Math.Pow(logarithmicBase, logMin + accDelta);
                accDelta += delta;// accDelta = delta * i
            }

            return v;
        }


        #endregion

        #region DEBUG
        public static void DebugFloatList(float[] values)
        {

            string s = "";

            for (int a = 0; a < values.Length; a++)
            {
                s += values[a].ToString() + " ";
            }

            Debug.Log(s);

        }

        #endregion

        #region CONVERSIONS

        public static float inchUnit = 0.0254f;

        public static float FromInchs(float value)
        {

            float result = value * inchUnit;

            return result;

        }

        public static float ToInchs(float value)
        {

            float result = value / inchUnit;

            return result;

        }

        #endregion

        #region RANGE


        public enum RangeFilterType
        {
            None,
            Centered,
            CenteredPerc,
            CenteredPercBounded

        }


        public static float[] GetSpreadedValues(float valueIn, int spreadCount)
        {
            float[] values = new float[spreadCount];
            float value = valueIn * spreadCount;

            for (int a = 0; a < values.Length; a++)
            {
                values[a] = RemapIntervalClip(value, a, a + 1, 0, 1);
            }

            return values;

        }

        [Serializable]
        public class RangeFilter
        {
            [SerializeField]
            public RangeFilterType Type;
            [SerializeField]
            public float Edge;
            [SerializeField]
            public Vector2 Bounds;

            [SerializeField]
            public float EdgeMinMultiplier;
            [SerializeField]
            public float EdgeMaxMultiplier;

            public RangeFilter(RangeFilterType type = RangeFilterType.None, float edge = 0, Vector2 bounds = default)
            {
                Type = type;
                Edge = edge;
                Bounds = bounds;

            }

        }

        [Serializable]
        public class FloatRange
        {
            [HorizontalGroup("Range")]
            [SerializeField]
            public float Min;
            [HorizontalGroup("Range")]
            [SerializeField]
            public float Max;

            public FloatRange(float rangeSize = 0)
            {

            }

            public bool ContainValue(float value)
            {
                bool result = false;

                if (value >= Min && value <= Max)
                {
                    result = true;
                }

                return result;

            }

            public bool ContainsValue(float value, RangeFilter filter = null)
            {
                bool result = false;

                if (filter == null)
                {
                    if (value >= Min && value <= Max)
                    {
                        result = true;
                    }
                }

                return result;

            }

        }

        [Serializable]
        public class FloatRangeDefault
        {
            [SerializeField]
            [MinMaxSlider(0, 1, true)]
            public Vector2 Interval;

            public bool ContainsValue(float value, RangeFilter filter = null)
            {
                bool result = false;

                //Debug.Log(value + " " + Interval.x + " " + Interval.y);

                if (value >= Interval.x && value <= Interval.y)
                {
                    result = true;
                }


                return result;

            }

        }

        public static bool IsFloatWithin(this float value, float min, float max)
        {
            return (value >= min && value < max);
        }

        public static int IndexWithinStackedFloatRange(int stacksCount, float rangeSize, float value, float baseValue = 0, RangeFilter filter = null)
        {
            int result = -1;
            int a = 0;

            //string debugString = " Value: " + value;

            float rangeSizeHalf = rangeSize / 2;

            float edge = 0;


            if (filter != null)
            {
                if (filter.Type == RangeFilterType.CenteredPercBounded)
                {
                    edge = rangeSize * filter.Edge;
                }
            }

            while (a < stacksCount)
            {

                FloatRange range = new FloatRange(rangeSize);

                range.Min = a * rangeSize;
                range.Max = range.Min + rangeSize;

                if (filter != null)
                {
                    if (filter.Type == RangeFilterType.CenteredPercBounded)
                    {

                        if (a == 0)
                        {
                            range.Min = baseValue;
                            range.Max = range.Min + rangeSizeHalf;

                        }
                        else if (a == stacksCount - 1)
                        {
                            range.Max = (a * rangeSize) + baseValue;
                            range.Min = range.Max - rangeSizeHalf;
                        }
                        else
                        {
                            range.Min = (rangeSizeHalf + rangeSize * (a - 1)) + edge + baseValue;
                            range.Max = (range.Min + rangeSize) - (edge * 2);
                        }
                    }

                }

                //debugString += "\r\n" + a + " - " + range.Min + "-" + range.Max;
                //Debug.Log("_____ Value :" + value + "Range: " + range.Min + "-" + range.Max + "Result "+ a);

                if (range.ContainValue(value))
                {

                    //Debug.Log("_____ Value :" + value + "   Range: " + range.Min + "-" + range.Max + "   Result " + a);
                    return a;
                    break;
                }

                a++;
            }

            //Debug.Log(debugString);

            return result;

        }

        public static int IndexWithinStackedFloatRange(int stacksCount = 1, int baseValue = 0, float value = 0, RangeFilter filter = null)
        {
            int result = -1;
            int a = 0;

            float rangeSize = (float)(1f / stacksCount);
            float edge = 0;

            while (a < stacksCount)
            {

                FloatRange range = new FloatRange(rangeSize);

                range.Min = a * rangeSize;
                range.Max = range.Min + rangeSize;

                if (filter != null)
                {
                    if (filter.Type == RangeFilterType.CenteredPercBounded)
                    {
                        edge = rangeSize * filter.Edge;

                        range.Min += edge;
                        range.Max -= edge;
                    }
                }

                //Debug.Log("_____ Value :" + value + "Range: " + range.Min + " - " + range.Max + "  StepsCount " + stacksCount + "  RangeSize " + rangeSize);

                if (range.ContainValue(value))
                {
                    //Debug.Log("_____ Value :" + value + "   Range: " + range.Min + "-" + range.Max + "   Result " + a);
                    return a + baseValue;
                }

                a++;
            }

            //Debug.Log(debugString);

            return result;

        }

        #endregion

        #region TRIGO

        public static Vector3 PointBetweenPoints(Vector3 pointA, Vector3 pointB, float distance = .5f, bool normalized = false)
        {

            Vector3 pos;

            Vector3 lineVector = pointB - pointA;
            if (normalized) lineVector = Vector3.Normalize(lineVector);

            pos = pointA + lineVector * distance;


            return pos;
        }



        public static Vector3 PointOnCircle(Vector3 center, float radius, float angle)
        {

            Vector3 pos;

            pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            pos.z = center.z;

            return pos;
        }

        public static Vector2 PointOnCircle(Vector2 center, float radius, float angle)
        {

            Vector2 pos;

            pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            //pos.z = center.z;

            return pos;
        }

        // point[1] = point[0] +(point[2] -point[0])/2 +Vector3.up *5.0f; // Play with

        public static float CalculateSine(float x, float t)
        {
            return Mathf.Sin(pi * (x + t));
        }

        public static float CalculateMultiSine(float x, float t)
        {
            float y = Mathf.Sin(pi * (x + t));
            y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
            y *= 2f / 3f;
            return y;
        }

        static float Sine2DFunction(float x, float z, float t)
        {
            return Mathf.Sin(pi * (x + z + t));
        }

        static float MultiSine2DFunction(float x, float z, float t)
        {
            float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
            y += Mathf.Sin(pi * (x + t));
            y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
            y *= 1f / 5.5f;
            return y;
        }

        static Vector3 MultiSine3DFunction(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
            p.y += Mathf.Sin(pi * (x + t));
            p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
            p.y *= 1f / 5.5f;
            p.z = z;
            return p;
        }

        static Vector3 Ripple(float x, float z, float t)
        {
            Vector3 p;
            float d = Mathf.Sqrt(x * x + z * z);
            p.x = x;
            p.y = Mathf.Sin(pi * (4f * d - t));
            p.y /= 1f + 10f * d;
            p.z = z;
            return p;
        }

        static Vector3 Sphere(float u, float v, float t)
        {
            Vector3 p;
            float r = Mathf.Cos(pi * 0.5f * v);
            p.x = r * Mathf.Sin(pi * u);
            p.y = v;
            p.z = r * Mathf.Cos(pi * u);
            return p;
        }

        static Vector3 Cylinder(float u, float v, float t)
        {
            Vector3 p;
            p.x = 0f;
            p.y = 0f;
            p.z = 0f;
            return p;
        }

        static Vector3 Torus(float u, float v, float t)
        {
            Vector3 p;
            float s = Mathf.Cos(pi * 0.5f * v);
            p.x = s * Mathf.Sin(pi * u);
            p.y = Mathf.Sin(pi * 0.5f * v);
            p.z = s * Mathf.Cos(pi * u);
            return p;
        }


        static Vector3 CalculateMultiSine(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin(pi * (x + t));
            p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
            p.y *= 2f / 3f;
            p.z = z;
            return p;
        }



        #endregion

        #region ANGLES

        public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
        {
            Vector3 right;
            if (clockwise)
            {
                right = Vector3.Cross(forward, axis);
                forward = Vector3.Cross(axis, right);
            }
            else
            {
                right = Vector3.Cross(axis, forward);
                forward = Vector3.Cross(right, axis);
            }
            return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
        }

        public static float ClampAngleNinja(float angle, float min, float max)
        {
            Debug.Log(angle + "   " + min + "  " + max);

            if ((angle <= 180 && angle >= 180 - Mathf.Abs(min)) || (angle >= 180 && angle <= 180 + max))
            {
                return Mathf.Clamp(angle, 180 - Mathf.Abs(min), 180 + max);
            }

            if (angle > 180f)
            {
                angle -= 360f;
            }

            angle = Mathf.Clamp(angle, min, max);

            if (angle < 0f)
            {
                angle += 360f;
            }

            Debug.Log(angle + "   " + min + "  " + max);

            return angle;
        }



        public static float NormalizedAngle(this float angle)
        {
            if (angle < -180F)
                angle += 360f;
            //if (angle > 180f) 
            if (angle > 360f)
                angle -= 360f;

            return angle;
        }

        public static float NormalizeAngle(float angle)
        {
            return angle.NormalizedAngle();
        }

        public static float ClampAngleNormalized(float angle, float min, float max)
        {

            float angleMin = Math.Min(min.NormalizedAngle(), max.NormalizedAngle());
            float angleMax = Math.Max(min.NormalizedAngle(), max.NormalizedAngle());

            //Debug.Log("_____  " + angle);

            //if (angle > 180f) // remap 0 - 360 --> -180 - 180
            //    angle -= 360f;
            angle = Mathf.Clamp(angle, angleMin, angleMax);
            if (angle < 0f) // map back to 0 - 360
                angle += 360f;

            //Debug.Log(angle + "   " + angleMin + " " + angleMax);

            return angle;
        }




        public static float ClampAngleBetterMore(float angle, float min, float max)
        {

            //float angleMin = Math.Min(min, max).NormalizedAngle();
            //float angleMax = Math.Max(min, max).NormalizedAngle();

            float angleMin = Math.Min(min, max);
            float angleMax = Math.Max(min, max);

            Debug.Log("_____  " + angle);

            // if (angle > 180f) // remap 0 - 360 --> -180 - 180
            //    angle -= 360f;
            angle = Mathf.Clamp(angle, angleMin, angleMax);
            if (angle < 0f) // map back to 0 - 360
                angle += 360f;

            Debug.Log(angle + " " + min + "  " + max + " " + angleMin + " " + angleMax);

            return angle;
        }

        public static float ClampAngleBetter(float angle, float min, float max)
        {

            //float angleMin = Math.Min(min, max).NormalizedAngle();
            //float angleMax = Math.Max(min, max).NormalizedAngle();

            float angleMin = Math.Min(min, max);
            float angleMax = Math.Max(min, max);

            Debug.Log("_____  " + angle);

            if (angle > 180f) // remap 0 - 360 --> -180 - 180
                angle -= 360f;
            angle = Mathf.Clamp(angle, angleMin, angleMax);
            if (angle < 0f) // map back to 0 - 360
                angle += 360f;

            Debug.Log(angle + "   " + angleMin + " " + angleMax);

            return angle;
        }

        public static float ClampAngle(float angle, float min, float max)
        {

            if (angle < 90 || angle > 270)
            {       // if angle in the critic region...
                if (angle > 180) angle -= 360;  // convert all angles to -180..+180
                if (max > 180) max -= 360;
                if (min > 180) min -= 360;
            }
            angle = Mathf.Clamp(angle, min, max);
            if (angle < 0) angle += 360;  // if angle negative, convert to 0..360

            return angle;
        }

        public static float ClampSimple(float angle)
        {

            Debug.Log("   " + angle);

            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;


            return angle;
        }


        #endregion

        #region INTERPOLATE

        public static float Interpolate(float alpha, float x0, float x1)
        {
            return x0 + ((x1 - x0) * alpha);
        }

        public static int InterpolateInt(int alpha, int x0, int x1)
        {
            return Mathf.Abs(x0 + ((x1 - x0) * alpha));
        }

        public static Vector3 Interpolate(float alpha, Vector3 x0, Vector3 x1)
        {
            return x0 + ((x1 - x0) * alpha);
        }


        public static float Clip(float x, float min, float max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }


        public static int ClipInt(int x, int min, int max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }


        public static float Wrap(float x, float rangeIn, float RangeOut)
        {

            float result = x;

            // uninterpolate: what is x relative to the interval in0:in1?
            if (x < rangeIn)
            {

                result = RangeOut + x;
            }

            if (x > RangeOut)
            {
                result = rangeIn + (x - RangeOut);
            }

            return result;


        }

        public static float RemapInterval(float x, float in0, float in1, float out0, float out1)
        {
            // uninterpolate: what is x relative to the interval in0:in1?
            float relative = (x - in0) / (in1 - in0);

            // now interpolate between output interval based on relative x
            return Interpolate(relative, out0, out1);
        }



        // ----------------------------------------------------------------------------
        // remap a value specified relative to a pair of bounding values
        // to the corresponding value relative to another pair of bounds.
        // Inspired by (dyna:remap-interval y y0 y1 z0 z1)
        public static float RemapIntervalWrap(float x, float in0, float in1, float out0, float out1)
        {
            // uninterpolate: what is x relative to the interval in0:in1?
            float relative = (x - in0) / (in1 - in0);

            // now interpolate between output interval based on relative x
            return Wrap(Interpolate(relative, out0, out1), out0, out1);
        }

        // Like remapInterval but the result is clipped to remain between
        // out0 and out1
        public static float RemapIntervalClip(float x, float in0, float in1, float out0, float out1, float offset=0)
        {
            // uninterpolate: what is x relative to the interval in0:in1?
            float relative = (x - in0) / (in1 - in0);

            // now interpolate between output interval based on relative x
            return Interpolate(Clip(relative, 0, 1), out0, out1)+offset;
        }

      

        public static float RemapIntervalClipInt(int x, int in0, int in1, int out0, int out1)
        {
            // uninterpolate: what is x relative to the interval in0:in1?
            int relative = (x - in0) / (in1 - in0);

            // now interpolate between output interval based on relative x
            return InterpolateInt(ClipInt(relative, 0, 1), out0, out1);
        }


        public static float RemapAngle(float angle, float minLimitIn, float maxLimitIn, float minLimitOut, float MaxLimitOut)
        {

            float result = angle >= 180f ? (angle - 360f) : angle;
            result = RemapIntervalClip(result, minLimitIn, maxLimitIn, minLimitOut, MaxLimitOut);

            return result;
        }

        /// BOH 
        /// 

        public static float Remap(float value, float from1, float to1, float from2, float to2, bool clamp = true)
        {

            float result = ((value - from1) / (to1 - from1) * (to2 - from2) + from2);

            if (clamp)
            {

                result = Mathf.Clamp(result, from2, to2);
            }

            return result;

        }

        #endregion

    }
}
