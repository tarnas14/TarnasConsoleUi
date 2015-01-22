namespace Tarnas.ConsoleUi
{
    using System.Collections.Generic;
    using System.Linq;

    public class StandardUserCommandFactory : UserCommandFactory
    {
        public UserCommand CreateUserCommand(string userInput)
        {
            var sanitizedInput = GetSanitizedInput(userInput);

            ValidateInput(sanitizedInput);

            var expressions = ExtractValidExpressions(sanitizedInput);

            var name = expressions.First();
            var flags = expressions.Skip(1).Where(IsFlag).ToList();
            flags.ForEach(flag => expressions.Remove(flag));
            var commandParams = expressions.Skip(1);

            return new UserCommand
            {
                Name = name,
                Params = commandParams.ToList(),
                Flags = flags.Select(flag => flag.Substring(2)).ToList()
            };
        }

        private static string GetSanitizedInput(string userInput)
        {
            return userInput.Trim();
        }

        private static List<string> ExtractValidExpressions(string sanitizedUserInput)
        {
            var withoutLeadingSlash = sanitizedUserInput.Substring(1);
            var betweenApostrophies = withoutLeadingSlash.Split('\'');
            var expressions = new List<string>();
            for (int i = 0; i < betweenApostrophies.Length; ++i)
            {
                if (IndexBetweenApostrophies(i))
                {
                    expressions.AddRange(betweenApostrophies[i].Trim().Split(' '));
                }
                else
                {
                    expressions.Add(betweenApostrophies[i].Trim());
                }
            }
            return expressions;
        }

        private static bool IndexBetweenApostrophies(int i)
        {
            return i%2 == 0;
        }

        private static void ValidateInput(string sanitizedUserInput)
        {
            if (string.IsNullOrWhiteSpace(sanitizedUserInput) ||
                HasNoCommand(sanitizedUserInput) ||
                HasEmptyCommandName(sanitizedUserInput))
            {
                throw new InvalidCommandStringException();
            }
        }

        private static bool HasNoCommand(string sanitizedUserInput)
        {
            return !sanitizedUserInput.StartsWith("/");
        }

        private static bool HasEmptyCommandName(string sanitizedUserInput)
        {
            return string.IsNullOrWhiteSpace(sanitizedUserInput[1].ToString());
        }

        private static bool IsFlag(string arg)
        {
            return arg.StartsWith("--");
        }
    }
}