using System;
using System.Collections;
using System.Collections.Generic;

public interface IDocument
{
    void Show();
}

public interface ITransaction
{
    decimal GetAmount();
}

public interface IInvoiceItem
{
    string GetProduct();
    int GetQuantity();
}

public class Document : IDocument
{
    protected string documentType;
    protected string issuer;
    protected string recipient;
    protected DateTime date;

    public Document(string documentType, string issuer, string recipient, DateTime date)
    {
        this.documentType = documentType;
        this.issuer = issuer;
        this.recipient = recipient;
        this.date = date;

        Console.WriteLine("Document constructor with parameters called.");
    }

    public Document() : this("DefaultType", "DefaultIssuer", "DefaultRecipient", DateTime.Now)
    {
        Console.WriteLine("Document default constructor called.");
    }

    public Document(string documentType) : this(documentType, "DefaultIssuer", "DefaultRecipient", DateTime.Now)
    {
        Console.WriteLine("Document constructor with document type called.");
    }

    ~Document()
    {
        Console.WriteLine("Document destructor called.");
    }

    public virtual void Show()
    {
        Console.WriteLine($"Document Type: {documentType}");
        Console.WriteLine($"Issuer: {issuer}");
        Console.WriteLine($"Recipient: {recipient}");
        Console.WriteLine($"Date: {date:d}");
    }
}

public class Receipt : Document, ITransaction
{
    private decimal amount;

    public Receipt(string issuer, string recipient, DateTime date, decimal amount)
        : base("Receipt", issuer, recipient, date)
    {
        this.amount = amount;

        Console.WriteLine("Receipt constructor called.");
    }

    public Receipt() : base()
    {
        Console.WriteLine("Receipt default constructor called.");
    }

    public Receipt(decimal amount) : base("Receipt")
    {
        this.amount = amount;
        Console.WriteLine("Receipt constructor with amount called.");
    }

    ~Receipt()
    {
        Console.WriteLine("Receipt destructor called.");
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Amount: {amount:C}");
    }

    public decimal GetAmount()
    {
        return amount;
    }
}

public class Invoice : Document, IInvoiceItem
{
    private string product;
    private int quantity;

    public Invoice(string issuer, string recipient, DateTime date, string product, int quantity)
        : base("Invoice", issuer, recipient, date)
    {
        this.product = product;
        this.quantity = quantity;

        Console.WriteLine("Invoice constructor called.");
    }

    public Invoice() : base()
    {
        Console.WriteLine("Invoice default constructor called.");
    }

    public Invoice(int quantity) : base("Invoice")
    {
        this.quantity = quantity;
        Console.WriteLine("Invoice constructor with quantity called.");
    }

    ~Invoice()
    {
        Console.WriteLine("Invoice destructor called.");
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Product: {product}");
        Console.WriteLine($"Quantity: {quantity}");
    }

    public string GetProduct()
    {
        return product;
    }

    public int GetQuantity()
    {
        return quantity;
    }
}

public class Bill : Document, ITransaction
{
    private decimal totalAmount;

    public Bill(string issuer, string recipient, DateTime date, decimal totalAmount)
        : base("Bill", issuer, recipient, date)
    {
        this.totalAmount = totalAmount;

        Console.WriteLine("Bill constructor called.");
    }

    public Bill() : base()
    {
        Console.WriteLine("Bill default constructor called.");
    }

    public Bill(decimal totalAmount) : base("Bill")
    {
        this.totalAmount = totalAmount;
        Console.WriteLine("Bill constructor with total amount called.");
    }

    ~Bill()
    {
        Console.WriteLine("Bill destructor called.");
    }

    public override void Show()
    {
        base.Show();
        Console.WriteLine($"Total Amount: {totalAmount:C}");
    }

    public decimal GetAmount()
    {
        return totalAmount;
    }
}

public interface ISoftware
{
    void DisplayInfo();
    bool IsUsable();
}

public abstract class Software : ISoftware
{
    protected string name;
    protected string manufacturer;

    public Software(string name, string manufacturer)
    {
        this.name = name;
        this.manufacturer = manufacturer;
    }

    public abstract void DisplayInfo();
    public abstract bool IsUsable();
}

public class FreeSoftware : Software, IComparable, IEnumerable<FreeSoftware>
{
    private List<FreeSoftware> softwareList;

    public FreeSoftware(string name, string manufacturer) : base(name, manufacturer)
    {
        softwareList = new List<FreeSoftware>();
        softwareList.Add(this);
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Manufacturer: {manufacturer}");
    }

    public override bool IsUsable()
    {
        return true;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        FreeSoftware otherSoftware = obj as FreeSoftware;
        if (otherSoftware != null)
            return this.name.CompareTo(otherSoftware.name);
        else
            throw new ArgumentException("Object is not a FreeSoftware");
    }

    public IEnumerator<FreeSoftware> GetEnumerator()
    {
        return softwareList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class ConditionalFreeSoftware : Software, IDisposable
{
    private DateTime installationDate;
    private TimeSpan trialPeriod;

    public ConditionalFreeSoftware(string name, string manufacturer, DateTime installationDate, TimeSpan trialPeriod) : base(name, manufacturer)
    {
        this.installationDate = installationDate;
        this.trialPeriod = trialPeriod;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Manufacturer: {manufacturer}");
        Console.WriteLine($"Installation Date: {installationDate:d}");
        Console.WriteLine($"Trial Period: {trialPeriod.Days} days");
    }

    public override bool IsUsable()
    {
        return DateTime.Now < installationDate + trialPeriod;
    }

    public void Dispose()
    {
        Console.WriteLine($"Disposing {name}...");
    }
}

public class CommercialSoftware : Software, IFormattable
{
    private decimal price;
    private DateTime installationDate;
    private TimeSpan usagePeriod;

    public CommercialSoftware(string name, string manufacturer, decimal price, DateTime installationDate, TimeSpan usagePeriod) : base(name, manufacturer)
    {
        this.price = price;
        this.installationDate = installationDate;
        this.usagePeriod = usagePeriod;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Manufacturer: {manufacturer}");
        Console.WriteLine($"Price: {price:C}");
        Console.WriteLine($"Installation Date: {installationDate:d}");
        Console.WriteLine($"Usage Period: {usagePeriod.Days} days");
    }

    public override bool IsUsable()
    {
        return DateTime.Now < installationDate + usagePeriod;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (format == "N")
            return $"{name} - {manufacturer}";
        else if (format == "P")
            return $"{price:C}";
        else
            throw new FormatException($"Invalid format string: {format}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        IDocument[] documents = new IDocument[]
        {
            new Receipt("Issuer1", "Recipient1", DateTime.Now, 100.50m),
            new Invoice("Issuer2", "Recipient2", DateTime.Now, "Product1", 5),
            new Bill("Issuer3", "Recipient3", DateTime.Now, 500.75m)
        };

        Console.WriteLine("Documents:");
        Console.WriteLine();

        foreach (var doc in documents)
        {
            doc.Show();
            Console.WriteLine();
        }

        Console.WriteLine("Transaction Amounts:");
        Console.WriteLine();

        foreach (var doc in documents)
        {
            if (doc is ITransaction transaction)
            {
                Console.WriteLine($"Amount: {transaction.GetAmount():C}");
            }
        }

        Software[] softwareDatabase = new Software[]
        {
            new FreeSoftware("Free Program", "Free Inc."),
            new ConditionalFreeSoftware("Trial Program", "Trial Ltd.", DateTime.Now.AddDays(-10), TimeSpan.FromDays(15)),
            new CommercialSoftware("Commercial Program", "Commercial Corp.", 99.99m, DateTime.Now.AddMonths(-2), TimeSpan.FromDays(90))
        };

        Console.WriteLine("Software Database:");
        Console.WriteLine();

        foreach (Software software in softwareDatabase)
        {
            software.DisplayInfo();
            Console.WriteLine($"Usable: {software.IsUsable()}");
            Console.WriteLine();
        }

        Console.WriteLine("Free Software List:");
        Console.WriteLine();

        FreeSoftware freeSoftware = new FreeSoftware("Free Program", "Free Inc.");

        foreach (var software in freeSoftware)
        {
            software.DisplayInfo();
            Console.WriteLine();
        }
    }
}
