namespace Fiit_passport.Models;

public interface IPassport
{
    public string SessionId { get; set; }
    public string OrdererName { get; set; }
    public string ProjectName { get; set; }
    public string ProjectDescription { get; set; }
    public string Goal { get; set; }
    public string Result { get; set; }
    public string AcceptanceCriteria { get; set; }
    public string CopiesNumber { get; set; }
    public string MeetingLocation { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}