using System;

namespace SpeakOut
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class UtteranceAttribute : Attribute
    {
        public UtteranceAttribute(string utterance)
        {
            this.Utterance = utterance;
        }

        public string Utterance { get; private set; }
    }
}