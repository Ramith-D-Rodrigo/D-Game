public class Mapper
{
    public static float Map(float value, float inMin, float inMax, float outMin, float outMax){
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
