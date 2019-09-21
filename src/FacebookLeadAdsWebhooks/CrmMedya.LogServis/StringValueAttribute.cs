using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class StringValueAttribute : Attribute
{
    private String description;
    private String language = "tr-TR";
    private Boolean exclude = false;

    /// <summary>
    /// Burda degerini aliyoruz.
    /// </summary>
    public String Description
    {
        get { return description; }
    }

    /// <summary>
    /// Dil secenegini burda tanimliyoruz. default tr-TR
    /// </summary>
    public String Language
    {
        get { return language; }
    }

    public Boolean Exclude
    {
        get { return exclude; }
        set { exclude = value; }
    }

    public Object Value { get; set; }

    public StringValueAttribute(String description)
    {
        this.description = description;
    }

    public StringValueAttribute(String description, String language)
    {
        this.description = description;
        this.language = language;
    }
}
