using System;
using System.Globalization;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace SemilleroGR3.Helpers
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var codigo = value?.ToString()?.ToUpper();

            return codigo switch
            {
                "I" => Color.FromArgb("#BDBDBD"),  // Iniciado: Gris
                "EP" => Color.FromArgb("#FFCA28"), // En Proceso: Amarillo cálido
                "L" => Color.FromArgb("#66BB6A"),  // Logrado: Verde
                "DL" => Color.FromArgb("#FFD700"), // Destacado: Dorado
                _ => Color.FromArgb("#E0E0E0")     // Gris claro por defecto
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}