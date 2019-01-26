using System;
using System.Collections.Generic;
using System.Linq;

public static class MetodosEstaticos
{
    public static List<T> RepetirPorPadrao<T>(this List<T> lista)
    {
        return Repetir(lista, default(T), lista.Capacity);
    }
    public static List<T> RepetirPorPadrao<T> (this List<T> lista, int count)
    {
        return Repetir(lista, default(T), count);
    }

    private static List<T> Repetir<T>(List<T> lista, T valor)
    {
        lista.AddRange(Enumerable.Repeat(valor, lista.Capacity));
        return lista;
    }

    private static List<T> Repetir<T>(List<T> lista, T valor, int count)
    {
        lista.AddRange(Enumerable.Repeat(valor, count));
        return lista;
    }
}
