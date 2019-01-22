using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace ITour.Models
{
    public class PrintTemplate
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Шаблон")]
        public string ContentTemplate { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; }

        [Display(Name = "Очередность")]
        public int Sequence { get; set; }


        public string GetContent(Order order)
        {
            PrintDictionary printDictionary = new PrintDictionary(order);
            string content = ContentTemplate ?? "";
            Regex regex = new Regex(@"\{\S+\}");
            MatchCollection matches = regex.Matches(content);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    if(printDictionary.ContainsKey(match.Value))
                        content = content.Replace(match.Value, printDictionary[match.Value]);
                }
            }
            return content;
        }
    }

    public class PrintDictionary : Dictionary<string, string>
    {
        public PrintDictionary(Order order) : base()
        {
            // Заказ
            Add("{Заказ.Номер}", order.Number?.ToString());
            Add("{Заказ.ДатаПечати}", order.DatePrintString);
            Add("{Заказ.Подтверждение}", order.ReservationNumber);
            Add("{Заказ.ОбщаяСтоимостьР}", order.OrderCostCurrency);
            Add("{Заказ.ОбщаяСтоимостьСтрока}", order.OrderCostString);
            Add("{Заказ.СписокТуристов}", order.TouristsString);
            Add("{Заказ.КоличествоТуристов}", order.TouristsCount?.ToString());
            Add("{Заказ.КоличествоДней}", order.CountDays?.ToString());
            Add("{Заказ.КоличествоНочей}", order.CountNights?.ToString());
            Add("{Заказ.ДатаНачала}", order.DateBeginString);
            Add("{Заказ.ДатаОкончания}", order.DateEndString);
            Add("{Заказ.Комментарий}", order.Comment);

            // Заказ.СписокУслуг
            var services = order.Services.OrderBy(s => s.Sequence);
            string servicesPrint = "";
            foreach (Service service in services)
            {
                servicesPrint += service.DetailsPrint;
            }
            Add("{Заказ.СписокУслуг}", servicesPrint);

            // Турагентство
            string agencyCompanyCommonInfo = $"<b>{order.AgencyCompany?.FullName}</b>, " +
                $"в лице {order.AgencyCompany?.PersonPost} <b>{order.AgencyCompany?.Person?.FullNameGenitive}</b>, " +
                $"действующего на основании {order.AgencyCompany?.ActionCause}. " +
                $"Юридический адрес: {order.AgencyCompany?.AddressLegal}. " +
                $"Адрес(место нахождения): {order.Manager?.AgencyOffice?.AddressPostal}. " +
                $"Банковские реквизиты: {order.AgencyCompany?.Bank}." +
                $"ИНН / КПП {order.AgencyCompany?.INN}, ОГРН {order.AgencyCompany?.OGRN}. " +
                $"тел: {order.Manager?.AgencyOffice?.Phone}. ";
            Add("{Турагентство.ОбщаяИнформация}", agencyCompanyCommonInfo);
            Add("{Турагентство.Наименование}", order.AgencyCompany?.Name);
            Add("{Турагентство.ПолноеНаименование}", order.AgencyCompany?.FullName);
            Add("{Турагентство.ЮридическийАдрес}", order.AgencyCompany?.AddressLegal);
            Add("{Турагентство.ИНН}", order.AgencyCompany?.INN);
            Add("{Турагентство.ОГРН}", order.AgencyCompany?.OGRN);
            Add("{Турагентство.БанковскиеРеквизиты}", order.AgencyCompany?.Bank);
            Add("{Турагентство.ФИОРуководителяПолностьюРП}", order.AgencyCompany?.Person?.FullNameGenitive);
            Add("{Турагентство.ДолжностьРуководителяРП}", order.AgencyCompany?.PersonPost);
            Add("{Турагентство.ДействующегоНаОсновании}", order.AgencyCompany?.ActionCause);


            // Турагентство.Офис (Офис Турагентства)
            Add("{Турагентство.Офис.ФактическийАдрес}", order.Manager?.AgencyOffice?.AddressPostal);
            Add("{Турагентство.Офис.Телефон}", order.Manager?.AgencyOffice?.Phone);
            Add("{Турагентство.Офис.Комментарий}", order.Manager?.AgencyOffice?.Comment);

            // Менеджер
            string powerAttorney = order.Manager?.PowerAttorneys?.FirstOrDefault() != null ?
                order.Manager?.PowerAttorneys?.FirstOrDefault().Print : "";
            Add("{Менеджер.Доверенность}", powerAttorney);

            // Заказчик
            Add("{Заказчик.Наименование}", order.Customer?.Name);
            Add("{Заказчик.ФИОПолностью}", order.Customer?.FullPersonName);
            
            // Заказчик ФизЛицо
            string customerPersonCommonInfo = $"<b>{order.Customer?.Person?.FullName}</b>. " +
                $"Основной документ: {order.Customer?.Person?.IdDocumentInfo}. " +
                $"Адрес места жительства(места пребывания): {order.Customer?.Person?.Address}, " +
                $"тел: <b>{order.Customer?.Person?.ApplicationUser?.PhoneNumber}</b> , " +
                $"email: {order.Customer?.Person?.ApplicationUser?.Email}. ";
            Add("{Заказчик.ФизЛицо.ОбщаяИнформация}", customerPersonCommonInfo);
            Add("{Заказчик.ФизЛицо.ФИОПолностью}", order.Customer?.Person?.FullName);
            Add("{Заказчик.ФизЛицо.ДокументИнфо}", order.Customer?.Person?.IdDocumentInfo);
            Add("{Заказчик.ФизЛицо.ТипДокумента}", order.Customer?.Person?.IdDocument?.DocumentType?.Name);
            Add("{Заказчик.ФизЛицо.СерияДокумента}", order.Customer?.Person?.IdDocument?.Series);
            Add("{Заказчик.ФизЛицо.НомерДокумента}", order.Customer?.Person?.IdDocument?.Number);
            Add("{Заказчик.ФизЛицо.КемВыданДокумент}", order.Customer?.Person?.IdDocument?.IssuedBy);
            Add("{Заказчик.ФизЛицо.КогдаВыданДокумент}", order.Customer?.Person?.IdDocument?.IssuedDateString);
            Add("{Заказчик.ФизЛицо.Адрес}", order.Customer?.Person?.Address);
            Add("{Заказчик.ФизЛицо.Телефон}", order.Customer?.Person?.ApplicationUser?.PhoneNumber);
            Add("{Заказчик.ФизЛицо.Email}", order.Customer?.Person?.ApplicationUser?.Email);

            // Заказчик ЮрЛицо
            string customerCustomerCompanyCommonInfo = $"<b>{order.Customer?.CustomerCompany?.FullName}</b> " +
                $"в лице {order.Customer?.CustomerCompany?.PersonPost} <b>{order.Customer?.Person?.FullNameGenitive}</b>, " +
                $"действующего на основании {order.Customer?.CustomerCompany?.ActionCause}. " +
                $"Юридический адрес: {order.Customer?.CustomerCompany?.AddressLegal}. " +
                $"Адрес(место нахождения): {order.Customer?.CustomerCompany?.AddressPostal}. " +
                $"ИНН/КПП {order.Customer?.CustomerCompany?.INN}, ОГРН {order.Customer?.CustomerCompany?.OGRN}. " +
                $"Банковские реквизиты: {order.Customer?.CustomerCompany?.Bank}. " +
                $"тел: <b>{order.Customer?.CustomerCompany?.Phone}</b>, " +
                $"email: {order.Customer?.CustomerCompany?.Email} ";
            Add("{Заказчик.ЮрЛицо.ОбщаяИнформация}", customerCustomerCompanyCommonInfo);
            Add("{Заказчик.ЮрЛицо.Наименование}", order.Customer?.CustomerCompany?.Name);
            Add("{Заказчик.ЮрЛицо.ПолноеНаименование}", order.Customer?.CustomerCompany?.FullName);
            Add("{Заказчик.ЮрЛицо.ФИОРуководителяПолностьюРП}", order.Customer?.Person?.FullNameGenitive);
            Add("{Заказчик.ЮрЛицо.ДолжностьРуководителяРП}", order.Customer?.CustomerCompany?.PersonPost);
            Add("{Заказчик.ЮрЛицо.ДействующегоНаОсновании}", order.Customer?.CustomerCompany?.ActionCause);
            Add("{Заказчик.ЮрЛицо.ЮридическийАдрес}", order.Customer?.CustomerCompany?.AddressLegal);
            Add("{Заказчик.ЮрЛицо.ФактическийАдрес}", order.Customer?.CustomerCompany?.AddressPostal);
            Add("{Заказчик.ЮрЛицо.ИНН}", order.Customer?.CustomerCompany?.INN);
            Add("{Заказчик.ЮрЛицо.ОГРН}", order.Customer?.CustomerCompany?.OGRN);
            Add("{Заказчик.ЮрЛицо.БанковскиеРеквизиты}", order.Customer?.CustomerCompany?.Bank);
            Add("{Заказчик.ЮрЛицо.Телефон}", order.Customer?.CustomerCompany?.Phone);
            Add("{Заказчик.ЮрЛицо.Email}", order.Customer?.CustomerCompany?.Email);

            // Туроператор    

            // ЮрЛицаТуроператора.ПолнаяИнформация
            string touroperatorsPrintFull = "";
            foreach (OrderTouroperatorCompany touroperator in order.Touroperators)
            {
                touroperatorsPrintFull += touroperator.TouroperatorCompany.FullInfo;
            }
            Add("{ЮрЛицаТуроператора.ПолнаяИнформация}", touroperatorsPrintFull);

            // ЮрЛицаТуроператора.КраткаяИнформация
            string touroperatorsPrintShort = "";
            foreach (OrderTouroperatorCompany touroperator in order.Touroperators)
            {
                touroperatorsPrintShort += touroperator.TouroperatorCompany.ShortInfo;
            }
            Add("{ЮрЛицаТуроператора.КраткаяИнформация}", touroperatorsPrintShort);
        }
    }
}
