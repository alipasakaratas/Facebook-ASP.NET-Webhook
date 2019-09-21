
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

public static class LogService
{
    public enum ItemTypes
    {
        [StringValue("Exception")]
        Exception = 1,

        [StringValue("Information")]
        Information = 2,

        [StringValue("Warning")]
        Warning = 3
    }

    #region Members

    private static String _logPath = AppDomain.CurrentDomain.BaseDirectory + "/LogFiles";
    private static String _userFullName = String.Empty;
    private static String _userMail = String.Empty;
    private static Int32 _userId = 0;
    private static ItemTypes _itemType = ItemTypes.Information;

    //private String _applicationName = AppDomain.CurrentDomain.FriendlyName;
    private static String _applicationName = "AlipasaCore";

    private static Object _syncRoot = new Object();

    #endregion Members

    #region Properties

    /// <summary>
    /// Gets and Sets
    /// </summary>
    public static string LogPath
    {
        get { return _logPath; }
        set { _logPath = value; }
    }

    /// <summary>
    /// Gets and Sets
    /// </summary>
    public static String UserFullName
    {
        get { return _userFullName; }
        set { _userFullName = value; }
    }

    /// <summary>
    /// Gets and Sets
    /// </summary>
    public static Int32 UserId
    {
        get { return _userId; }
        set { _userId = value; }
    }

    /// <summary>
    /// Gets and Sets
    /// </summary>
    public static String UserMail
    {
        get { return _userMail; }
        set { _userMail = value; }
    }

    /// <summary>
    /// Gets and Sets
    /// </summary>
    public static ItemTypes ItemType
    {
        get { return _itemType; }
        set { _itemType = value; }
    }

    /// <summary>
    /// Gets and Sets
    /// </summary>
    public static String ApplicationName
    {
        get
        {
            return _applicationName;
        }
        set
        {
            _applicationName = value;
        }
    }

    #endregion Properties

    /// <summary>
    ///
    /// </summary>
    /// <param name="location">hatanin yasandigi method'un adi yazilir</param>
    /// <param name="friendlyMessage">hata hakkinda bilgi yazilir</param>
    /// <param name="ex">gerceklesen exception yazilir.</param>
    public static void Save(String location, String friendlyMessage, Exception ex, ItemTypes type)
    {
        try
        {
            lock (_syncRoot)
            {
                if (!Directory.Exists(_logPath))
                    Directory.CreateDirectory(_logPath);

                List<Exception> list = new List<Exception>();
                if (ex != null)
                    GetRecursiveExceptionMessageList(ex, ref list);
                else
                    list.Add(new Exception("No Exception"));

                XElement element = new XElement("LogItem",
                new XElement("User",
                new XElement("Id", UserId),
                new XElement("Mail", UserMail),
                new XElement("FullName", UserFullName)
                        ),
                    new XElement("ItemType", type),
                    new XElement("Date", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffffff")),
                    new XElement("Location", location),
                    new XElement("FriendlyMessage", friendlyMessage),
                    new XElement("Exception",
                        from s in list
                        select new XElement("Messages", s.Message),
                        from s in list
                        select new XElement("StackTraces", s.StackTrace)
                        )

                        );

                String fileName = String.Format("Log_{0}_{1}.xml",
                    ApplicationName,
                    CreateFileName());

                String filePath = String.Format("{0}\\{1}",
                                        _logPath,
                                        fileName);

                XDocument doc = null;

                // check whether file exist
                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                }
                else
                {
                    doc = new XDocument();
                    doc.Add(new XElement("LogItemList"));
                }

                doc.Element("LogItemList").AddFirst(element);
                doc.Save(filePath);
            }
        }
        catch
        { }
    }

    public static void Save(String location, String friendlyMessage, string jsonItem, Exception ex, ItemTypes type)
    {
        try
        {
            lock (_syncRoot)
            {
                if (!Directory.Exists(_logPath))
                    Directory.CreateDirectory(_logPath);

                List<Exception> list = new List<Exception>();
                if (ex != null)
                    GetRecursiveExceptionMessageList(ex, ref list);
                else
                    list.Add(new Exception("No Exception"));

                XElement element = new XElement("LogItem",
                new XElement("User",
                new XElement("Id", UserId),
                new XElement("Mail", UserMail),
                new XElement("FullName", UserFullName)
                        ),
                    new XElement("ItemType", type),
                    new XElement("Date", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffffff")),
                    new XElement("Location", location),
                    new XElement("FriendlyMessage", friendlyMessage),
                          new XElement("JsonItem", jsonItem),
                    new XElement("Exception",
                        from s in list
                        select new XElement("Messages", s.Message),
                        from s in list
                        select new XElement("StackTraces", s.StackTrace)
                        )

                        );

                String fileName = String.Format("Log_{0}_{1}.xml",
                    ApplicationName,
                    CreateFileName());

                String filePath = String.Format("{0}\\{1}",
                                        _logPath,
                                        fileName);

                XDocument doc = null;

                // check whether file exist
                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                }
                else
                {
                    doc = new XDocument();
                    doc.Add(new XElement("LogItemList"));
                }

                doc.Element("LogItemList").AddFirst(element);
                doc.Save(filePath);
            }
        }
        catch
        { }
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="location">hatanin yasandigi method'un adi yazilir</param>
    /// <param name="friendlyMessage">hata hakkinda bilgi yazilir</param>
    /// <param name="ex">gerceklesen exception yazilir.</param>
    public static void Save(String location, String friendlyMessage, Exception ex, ItemTypes tip, string gonderilemeyenData)
    {
        try
        {
            lock (_syncRoot)
            {
                if (!Directory.Exists(_logPath))
                    Directory.CreateDirectory(_logPath);

                List<Exception> list = new List<Exception>();
                if (ex != null)
                    GetRecursiveExceptionMessageList(ex, ref list);
                else
                    list.Add(new Exception("No Exception"));

                XElement element = new XElement("LogItem",
                new XElement("User",
                new XElement("Id", UserId),
                new XElement("Mail", UserMail),
                new XElement("FullName", UserFullName)
                        ),
                    new XElement("ItemType", tip),
                    new XElement("Date", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.ffffff")),
                    new XElement("Location", location),
                    new XElement("FriendlyMessage", friendlyMessage),
                    new XElement("Exception",
                        from s in list
                        select new XElement("Messages", s.Message),
                        from s in list
                        select new XElement("StackTraces", s.StackTrace)
                        )

                        );

                String fileName = String.Format("Log_{0}_{1}.xml",
                    ApplicationName,
                    CreateFileName());

                String filePath = String.Format("{0}\\{1}",
                                        _logPath,
                                        fileName);

                XDocument doc = null;

                // check whether file exist
                if (File.Exists(filePath))
                {
                    doc = XDocument.Load(filePath);
                }
                else
                {
                    doc = new XDocument();
                    doc.Add(new XElement("LogItemList"));
                }

                doc.Element("LogItemList").AddFirst(element);
                doc.Save(filePath);

                if (tip == ItemTypes.Warning)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("Çıkan Hata");
                    sb.Append("_________________________________________________________________________________");

                    if (ex != null)
                    {
                        sb.AppendFormat("Inner Exception : {0}", ex.InnerException);
                        sb.AppendFormat("Exception : {0}", ex.InnerException);
                        sb.Append("_________________________________________________________________________________");
                    }

                    sb.Append("Gönderilemeyen Data");
                    sb.Append("_________________________________________________________________________________");
                    sb.Append(gonderilemeyenData);
                    sb.Append("_________________________________________________________________________________");

                }
            }
        }
        catch
        { }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="location">hatanin yasandigi method'un adi yazilir</param>
    /// <param name="friendlyName">hata hakkinda bilgi yazilir</param>
    public static void Save(String location, String friendlyName,ItemTypes tip)
    {
        Save(location, friendlyName, null, tip);
    }

    #region Privates

    private static void GetRecursiveExceptionMessageList(Exception ex, ref List<Exception> list)
    {
        list.Add(ex);
        if (ex.InnerException != null)
            GetRecursiveExceptionMessageList(ex.InnerException, ref list);
    }

    private static String CreateFileName()
    {
        Int32 day = DateTime.Now.Day;
        Int32 month = DateTime.Now.Month;
        Int32 year = DateTime.Now.Year;

        StringBuilder sb = new StringBuilder();
        String temp = Convert.ToString(year);
        sb.Append(temp.Substring(2));
        if (month < 10)
            sb.Append("0" + month);
        else
            sb.Append(month);

        if (day < 10)
            sb.Append("0" + day);
        else
            sb.Append(day);

        return sb.ToString();
    }

    #endregion Privates
}