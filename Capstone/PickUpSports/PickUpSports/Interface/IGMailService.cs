

namespace PickUpSports.Interface
{
    public interface IGMailService
    {
        bool Send(string body, string toEmailAddress);

    }
}