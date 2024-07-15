using System;

namespace Fanex.WCLeague.Business.Services
{
    public interface DateService
    {
        DateTime DateStart { get; }

        DateTime DateEnd { get; }
    }
}