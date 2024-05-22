namespace Fiit_passport.Models.Interfaces;

public interface IComment
{
    public string SessionId { get; set; }
    public string FieldName { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }
    public string TextComment { get; set; }
}