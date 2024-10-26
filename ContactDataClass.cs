using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Ecoplaza
{
    public class ContactDataClass
    {
        // Основные настройки
        public static string Brand() => "Экологические услуги.рф"; // Брэнд
        public static string LegalEntity() => "ИП Шелковников Я.Н."; // Наименование юридического лица
        public static string DetailsINN() => "710700417902"; // ИНН
        public static string DetailsOGRN() => "320715400012888"; // ОГРН
        public static string URL() => "https://экологические-услуги.рф"; // Полный URL со слешем в конце (для кириллических доменов не использовать)
        public static string URLshort() => "экологические-услуги.рф"; // Сокращённый URL на латинице (для использования в контенте)
        public static string CopyrightYears() => "2013–" + @DateTime.Now.Year.ToString(); // Год, начиная с которого действует авторское право (вычисляет период по текущий год)

        // Контактная информация
        public static string Mail() => "info@ecoplaza.pro"; // Электронная почта сайта
        public static string Phone() => "+7 (905) 118-18-56"; // Номер телефона для отображения
        public static string PhoneShort() => "+7 (905) Показать"; // Скрытый номер телефона для отображения
        public static string Skype() => "Skype:drshelkovnikov?chat"; // Логин Skype
                
        // Настройка форм обратной связи
        public static string EmailTo() => "info@ecoplaza.pro"; // Email получателя
        public static string UserName() => "info@ecoplaza.pro"; // Имя пользователя
        public static string Password() => "пароль"; // Пароль
        public static string Host() => "smtp.beget.com"; // Хост
        public static int Port() => 465; // Порты: 465 или 587
        public static bool Ssl() => true; // SSL: true или false

        // Локация
        public static string AddressLetter() => "300911, г. Тула, мкр Северный, д.13, кв.1"; // Полный адрес
        public static string AddressShrot() => "г. Тула"; // Короткий адрес
        public static string OpenHours() => "8:00-19:00 (мск.) | Пн.-Пт."; // Режим работы по умолчанию
        public static string OpenHoursMData() => "Mo-Fr 08:00-19:00"; // Микроданные режима работы по умолчанию
        public static string AddressYandexMaps() => "https://yandex.ru/maps/15/tula/house/ulitsa_generala_margelova_5/Z04YcwBjT0QPQFtufX11d3RhYA==/?from=tabbar&ll=37.573617%2C54.146824&source=serp_navig&z=17.09"; // Локация на Яндекс картах
        public static string AddressGoogleMaps() => "https://www.google.com/maps/d/embed?mid=1f238-JPvp__JN65-eGXCfG45I8hMZiE&ehbc=2E312F"; // Локация на Google картах

        // Автоформатирование - не изменять!
        public static string PhoneHref() => ("tel:+" + Regex.Replace(Phone(), @"\D+", "")); // Номер телефона для ссылки
        public static string Mes() => Regex.Replace(Phone(), @"\D+", ""); // Номер телефона для мессенджеров (без плюса)
        public static string WhatsApp() => ("https://wa.me/" + Regex.Replace(Phone(), @"\D+", "")); // Номер телефона для Whatsapp
        public static string Telegram() => ("https://t.me/share/url?text=&url=tel:" + Regex.Replace(Phone(), @"\D+", "")); // Номер телефона для Telegram
        public static string MailHref() => ("mailto:" + Mail()); // Электронная почта для ссылки
        
        
            // Преобразуем хост в Punycode (если латиница - вернет тоже самое)
            // var idn = new IdnMapping();
            //string punycodeHost = idn.GetAscii(host);
       

        
    }
}
