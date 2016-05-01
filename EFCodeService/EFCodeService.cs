using System;
using System.Data.Entity;
using System.Diagnostics;

namespace EFCodeService
{
    public class EFCodeService
    {
        internal const int MaxStringLength = 10;
        const int MAX_TRIES = 50;
        readonly DbContext _context;
        readonly int _codeLength;
        public EFCodeService(DbContext context, int codeLength)
        {
            _context = context;
            _codeLength = codeLength;
        }

        public void Populate(string title, Action<string> code, Action<string> slug)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("title");

            code(GenerateCode());
            slug(GenerateSlug(title));
        }

        public string GenerateSlug(string title)
        {
            return title.GenerateSlug();
        }

        public string GenerateCode()
        {
            var dbset = _context.Set(typeof(EFCode));

            Random random = new Random();
            int counter = 0;
            while (true)
            {
                var code = Utilities.GenerateRandomString(_codeLength, random);
                Debug.WriteLine(code);
                dbset.Add(new EFCode { Code = code });
                try
                {
                    _context.SaveChanges();
                    return code;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    if (counter++ > MAX_TRIES)
                        throw new ApplicationException("Could not generate random code. Database issue?");
                }
            }
        }
    }
}
