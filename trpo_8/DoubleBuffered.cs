using System.Windows.Forms;

public static class ControlExtensions
{
    public static void DoubleBuffered(this Control control, bool enable)
    {
        System.Reflection.PropertyInfo propertyInfo = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        propertyInfo.SetValue(control, enable, null);
    }
}
