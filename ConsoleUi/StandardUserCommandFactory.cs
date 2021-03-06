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

            var commandParams = expressions.Skip(1).ToList();

            var flags = ExtractFlags(commandParams);

            var namedParams = ExtractNamedParams(commandParams);

            return new UserCommand
            {
                Name = name,
                Params = commandParams.ToList(),
                Flags = flags,
                NamedParams = namedParams
            };
        }

        private IDictionary<string, string> ExtractNamedParams(IList<string> commandParams)
        {
            var paramNames = commandParams.Where(param => param.StartsWith("-"));
            
            var namedParamsDictionary = new Dictionary<string, string>();

            paramNames.ToList().ForEach(dashedParamName =>
            {
                string paramName = dashedParamName.Substring(1);
                var index = commandParams.IndexOf(dashedParamName);

                namedParamsDictionary.Add(paramName, commandParams[index + 1]);
                commandParams.RemoveAt(index);
                commandParams.RemoveAt(index);
            });

            return namedParamsDictionary;
        }

        private static IList<char> ExtractFlags(List<string> expressions)
        {
            var flagExpressions = expressions.Where(IsFlag).ToList();
            flagExpressions.ForEach(flag => expressions.Remove(flag));

            var flags = new List<char>();
            flagExpressions.ToList().ForEach(flagExpression => flags.AddRange(flagExpression.Substring(2).ToCharArray()));

            return flags;
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
                HasEmptyCommandName(sanitizedUserInput))
            {
                throw new InvalidCommandStringException();
            }

            if (HasNoCommandPrefix(sanitizedUserInput))
            {
                throw new NotACommandException();
            }
        }

        private static bool HasNoCommandPrefix(string sanitizedUserInput)
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