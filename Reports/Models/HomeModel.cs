using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FarmSib.Base.Data;

namespace FarmSib.Base.Models
{
    public class ManagerData
    {
        public String ManagerFilter { get; set; }
        public String ManagerSelector { get; set; }
        public String[] ManagerMultiSelector { get; set; }
        public List<SelectListItem> ManagerSelectList { get; set; }
        public MultiSelectList ManagerMultiSelectList { get; set; }
        public ManagerData()
        {
            ManagerSelectList = new List<SelectListItem>(0);
            ManagerMultiSelectList = new MultiSelectList(ManagerSelectList, "Value", "Text");
        }
        public void FillManagerData()
        {
            DataTable dt = HomeModel.GetManagerData(ManagerFilter);

            ManagerSelectList = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Value = (String)dr[3];
                item.Text = ((String)dr[4]).Trim();
                ManagerSelectList.Add(item);
            }
            ManagerMultiSelectList = new MultiSelectList(ManagerSelectList, "Value", "Text");
        }
    }
    public class ClientData
    {
        public String ClientFilter { get; set; }
        public String ClientSelector { get; set; }
        public List<SelectListItem> ClientSelectList { get; set; }
        public ClientData()
        {
            ClientSelectList = new List<SelectListItem>(0);
        }
        public void FillClientData()
        {
            DataTable dt = HomeModel.GetClientData(ClientFilter);

            ClientSelectList = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Value = (String)dr["CODE"];
                item.Text = ((String)dr["SP56"]).Trim() + " " + ((String)dr["DESCR"]).Trim();
                ClientSelectList.Add(item);
            }
        }
    }
    public static class HomeModel
    {
        public static DataTable GetClientData(String clientFilter)
        {
            return HomeData.Home.GetClientData(clientFilter);
        }
        public static DataTable GetManagerData(String managerFilter)
        {
            DataTable dt = HomeData.Home.GetManagerData(managerFilter);
            DataView dv = dt.DefaultView;
            dv.RowFilter = "DESCR <> ''";
            return dv.ToTable();
        }
    }

    public class CommonModel
    {
        public NskdSessionLite Session;
        public Object PageModel;
        public UserMainMenu UserMainMenu;
    }
    public class UserMainMenu
    {
        public String SelectedNodePath { get; set; }
        public String JsonUserMainMenu { get; set; }
        public UserMainMenu()
        {
            SelectedNodePath = "[Пользователь: ?]";
            JsonUserMainMenu = @"
                { name: 'Пользователь: ?', cont: [
                    { name: 'Регистрация', cont: [
                        { name: 'Вход' },
                        { name: 'Выход' } ] } ]
                }";
        }
        public UserMainMenu(NskdSessionLite session)
        {
            SelectedNodePath = "[" + session.UserName + "]";
            switch (session.UserId)
            {
                case 2: // Соколов Евгений Анатольевич - программист
                case 3: // Шанин Григорий Олегович - управляющий
                case 4: // Мартовицкий Дмитрий Владимрович - ген. директор
                case 6: // Родоманченко Наталья Витальевна - отд. кадров
                case 14: // Максутов Игорь
                case 26: // Мурзина Татьяна - секретарь
                case 60: // Миловидов Василий Александрович
                case 63: // Воронов Максим Владимирович - склад
                case 65: // Мурашова Т. Н. - склад
                case 66: // Ястребова Елена - зам. Шанина
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Сообщения (1)', url: null, cont: [
                                { name: 'Обязательные для ознакомления (1)', url: null },
                                { name: 'Информационные', url: null } ] },
                            { name: 'Документы 1с', url: null, cont: [
                                { name: 'Расходная (трек)', url: '/Docs1c/F0' },
                                { name: 'Расх. ==> Прих.', url: '/ImEx/F0' } ] },
                            { name: 'Заявки', url: null, cont: [
                                { name: 'Список', url: '/Prep/F0' } ] },
                            { name: 'Договоры', url: '/Agrs/F0', cont: [] },
                            { name: 'Закупки', url: null, cont: [
                                { name: 'Спецификации', url: null, cont: [
                                    { name: 'Список', url: '/Order/F0' },
                                    { name: 'Загрузка', url: '/Order/F3' } ] },
                                { name: 'Уведомления', url: null } ] },
                            { name: 'Документы', url: null, cont: [
                                { name: 'РУ', url: '/Docs/Rd' },
                                { name: 'СТ-1', url: '/Docs/Ct' } ] },
                            { name: 'Отчёты', url: null, cont: [
                                { name: 'Неоплаченные отгрузки', url: '/Reports/R1' },
                                { name: 'Отчёт по оплатам', url: '/Reports/R2' },
                                { name: 'Судебные документы', url: '/Reports/R3' },
                                { name: 'Обеспечение', url: null, cont: [
                                    { name: 'Долги по клиентам', url: 'Deposits/F0' },
                                    { name: 'Расчеты по аукционам', url: 'Deposits/F1' } ] } ] },
                            { name: 'Номенклатура', url: null, cont: [
                                { name: 'Препараты', url: '/Items/Search' },
                                { name: 'Группы', url: '/Items/Groups' } ] },
                            { name: 'Настройки', url: '/Settings/F0' },
                            { name: 'ExternalPages', url: '/ExternalPages'} ]
                        }";
                    break;
                case 5:  // Пирожкова Вероника - отдел продаж помошница Заваловой Елены
                case 13: // Коледова Юлия Ивановна - бывший регистратор теперь помощник Августовой Ангелины
                case 17: // Магергут Татьяна - менеджер по продажам
                case 18: // Скворцова Марина - менеджер по продажам
                case 19: // Сущева Ольга - менеджер по продажам
                case 21: // Романова Нина - менеджер по продажам
                case 22: // Каблукова М.
                case 23: // Завалова Елена - менеджер по продажам
                case 24: // Борисова Валентина
                case 25: // Миронова Кристина - менеджер по продажам
                case 27: // Волостных Роман - менеджер по продажам
                case 28: // Горинова Анастасия
                case 29: // Борисяк Ольга - менеджер по продажам
                case 30: // Ерастова Людмила - менеджер по продажам
                case 31: // Августова Ангелина - менеджер по продажам
                case 32: // Королькова Анна - менеджер по продажам
                case 33: // Шаповалова Валентина
                case 35: // Шанина Елена - менеджер по продажам - помощник Сущевой Ольги
                case 36: // Кравчук Ирина - менеджер по продажам - помощник Заваловой Елены
                case 37: // Алдущенкова Эльвира - менеджер по продажам - помощник Магергут Татьяны
                case 38: // Коробкова Юля - менеджер по продажам - помощник Корольковой Анны
                case 39: // Синицкая Иветта - менеджер по продажам - помощник Августовой Ангелины
                case 40: // Легонькова Анастасия - менеджер по продажам - помощник Заваловой Елены (Горинова?)
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 55: // Паннафидина Екатерина - помощник менеджера Ерастовой Людмилы
                case 56: // Серкерова
                case 57: // Тарунтаева - помощник Мироновой
                case 58: // Лобанова Елена - помощник Магергут Татьяны
                case 59: // Углова Алёна Александрована - помощник менеджера Корольковой Анны
                case 61: // Морева Марина - помощник Августовой Ангелины
                case 62: // Перевалова Юлия Викторовна - менеджер
                case 64: // Мехрабова
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Сообщения (1)', url: null, cont: [
                                { name: 'Обязательные для ознакомления (1)', url: null },
                                { name: 'Информационные', url: null } ] },
                            { name: 'Заявки', url: null, cont: [
                                { name: 'Список', url: '/Prep/F0' } ] },
                            { name: 'Закупки', url: null, cont: [
                                { name: 'Спецификации', url: null, cont: [
                                    { name: 'Список', url: '/Order/F0' },
                                    { name: 'Загрузка', url: '/Order/F3' } ] },
                                { name: 'Уведомления', url: null } ] },
                            { name: 'Отчёты', url: null, cont: [
                                { name: 'Неоплаченные отгрузки', url: '/Reports/R1' },
                                { name: 'Отчёт по оплатам', url: '/Reports/R2' },
                                { name: 'Судебные документы', url: '/Reports/R3' },
                                { name: 'Обеспечение', url: null, cont: [
                                    { name: 'Долги по клиентам', url: 'Deposits/F0' },
                                    { name: 'Расчеты по аукционам', url: 'Deposits/F1' } ] } ] },
                            { name: 'Препараты', url: '/Items/Search' },
                            { name: 'Настройки', url: '/Settings/F0' } ]
                        }";
                    break;
                case 7: // Сорокина Надежда Анатольевна - тендерный отдел (договоры)
                case 34: // Егорова Евгения Валерьевна - тендерный отдел
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Сообщения (1)', url: null, cont: [
                                { name: 'Обязательные для ознакомления (1)', url: null },
                                { name: 'Информационные', url: null } ] },
                            { name: 'Договоры', url: '/Agrs/F0', cont: [] },
                            { name: 'Заявки', url: null, cont: [
                                { name: 'Список', url: '/Prep/F0' } ] },
                            { name: 'Закупки', url: null, cont: [
                                { name: 'Спецификации', url: null, cont: [
                                    { name: 'Список', url: '/Order/F0' },
                                    { name: 'Загрузка', url: '/Order/F3' } ] },
                                { name: 'Уведомления', url: null } ] },
                            { name: 'Отчёты', url: null, cont: [
                                { name: 'Неоплаченные отгрузки', url: '/Reports/R1' },
                                { name: 'Отчёт по оплатам', url: '/Reports/R2' },
                                { name: 'Судебные документы', url: '/Reports/R3' },
                                { name: 'Обеспечение', url: null, cont: [
                                    { name: 'Долги по клиентам', url: 'Deposits/F0' } ] } ] },
                            { name: 'Настройки', url: '/Settings/F0' } ]
                        }";
                    break;
                case 8: // Максимова Екатерина Викторовна - юр. отдел
                case 9: // Бельченко Юлия Викторовна - юр. отдел
                case 10: // Федущак Роман Владимирович - юр. отдел
                case 20: // Потекаева Ирина Ивановна - юр. отдел
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Сообщения (1)', url: null, cont: [
                                { name: 'Обязательные для ознакомления (1)', url: null },
                                { name: 'Информационные', url: null } ] },
                            { name: 'Договоры', url: '/Agrs/F0', cont: [] },
                            { name: 'Документы 1с', url: null, cont: [
                                { name: 'Расходная (трек)', url: '/Docs1c/F0' } ] },
                            { name: 'Отчёты', url: null, cont: [
                                { name: 'Неоплаченные отгрузки', url: '/Reports/R1' },
                                { name: 'Отчёт по оплатам', url: '/Reports/R2' },
                                { name: 'Судебные документы', url: '/Reports/R3' } ] },
                            { name: 'Настройки', url: '/Settings/F0' } ]
                        }";
                    break;
                case 11: // Баржина Татьяна - секретарь
                case 12: // Баризова Наталья - секретарь
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Сообщения (1)', url: null, cont: [
                                { name: 'Обязательные для ознакомления (1)', url: null },
                                { name: 'Информационные', url: null } ] },
                            { name: 'Документы 1с', url: null, cont: [
                                { name: 'Расходная (трек)', url: '/Docs1c/F0' } ] },
                            { name: 'Настройки', url: '/Settings/F0' } ]
                        }";
                    break;
                case 54: // Михайлова Анна Андреевна - ассистент отдела закупок
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Сообщения (1)', url: null, cont: [
                                { name: 'Обязательные для ознакомления (1)', url: null },
                                { name: 'Информационные', url: null } ] },
                            { name: 'Документы', url: null, cont:[
                                { name: 'РУ', url: '/Docs/Rd' },
                                { name: 'СТ-1', url: '/Docs/Ct' } ] },
                            { name: 'Препараты', url: '/Items/Search' },
                            { name: 'Настройки', url: '/Settings/F0' } ]
                        }";
                    break;
                case 0: // Не прошел проверку
                case 1: // Пустой
                case 15: // Заколодкин Владимир
                case 16: // Кодина Марина
                default: // Все остальные
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Препараты', url: '/Items/Search' } ]
                        }";
                    break;
            }
        }
    }

    public class NskdSessionLite
    {
        public Guid SessionId;
        public Int32 UserId;
        public String UserName;
        public NskdSessionLite() { }
        public NskdSessionLite(String userHostAddress)
        {
            SessionId = Guid.NewGuid();
            UserId = 0;
            // регистрируем новую сессию пока без ключа и без пользователя
            HomeData.Home.CreateSession(SessionId, userHostAddress);
        }
        public void UpdateSession(String userToken, String cryptKey)
        {
            HomeData.Home.UpdateSession(userToken, SessionId, cryptKey);
        }
        public static NskdSessionLite GetById(Guid sessionId)
        {
            NskdSessionLite session = new NskdSessionLite();
            // загружаем данные сессии
            DataTable dt = HomeData.Home.GetSessionById(sessionId);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    session.SessionId = (Guid)dr["id"];
                    session.UserId = (dr["user_id"] == DBNull.Value) ? 0 : (Int32)dr["user_id"];
                    session.UserName = (dr["name"] == DBNull.Value) ? "Гость" : (String)dr["name"];
                }
            }
            return session;
        }
    }

}