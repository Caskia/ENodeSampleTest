using ENode.Commanding;
using ENode.EQueue;

namespace SendCommand.Tests
{
    public class CommandTopicProvider : AbstractTopicProvider<ICommand>
    {
        public override string GetTopic(ICommand command)
        {
            return "NoteCommandTopic";
        }
    }
}