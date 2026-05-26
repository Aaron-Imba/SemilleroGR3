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
            if (value is string codigoLogro)
            {
                // Evaluamos los códigos definidos en la base de datos (Tabla NivelLogro)
                return codigoLogro.ToUpper() switch
                {
                    "1" => Colors.LightPink,  // Iniciado (Borde rojo/rosa)
                    "EP" => Colors.Khaki,     // En Proceso (Borde amarillo)
                    "L" => Colors.LightGreen, // Logrado (Borde verde)
                    _ => Colors.LightGray     // Por defecto o sin evaluar
                };
            }

            return Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Como la vista de la familia es de solo lectura, no necesitamos convertir de UI a Datos
            throw new NotImplementedException();
        }
    }
}