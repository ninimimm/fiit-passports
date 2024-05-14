namespace Fiit_passport.Models.Interfaces;

public interface ISessionNumbers
{
    public string SessionId { get; set; }
    public int Number { get; set; }
    public Status Status { get; set; }
    public string Name { get; set; }
}