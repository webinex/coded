namespace Webinex.Coded
{
    public static class CodedResultExtensions
    {
        /// <summary>
        ///     Throws exception when <paramref name="result"/> failed
        /// </summary>
        /// <param name="result">Result to check</param>
        /// <exception cref="CodedException">Would be thrown if <paramref name="result"/> failed</exception>
        public static CodedResult Throw(this CodedResult result)
        {
            if (result.Succeed)
                return result;
            
            throw new CodedException(result.Failure);
        }
    }
}