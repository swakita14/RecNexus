

namespace PickUpSports.Interface
{
    public interface IGMailer
    {
        bool Send(string body, string toEmailAddress);

    }
}