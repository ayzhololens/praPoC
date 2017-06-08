class Loom {
    public static string apiKey = "2E6OKx3Cvk5zqi4eNccIL9";
    public static string apiStyle = "loomie";
    public static string licenseKey = "d5c4a8eb74b66b6185bd931a0dcb0d4f42f82a88e42be740ca143e";
    public static bool apiFacs = true;

	public static Tuple<string, string>[] meshMapping =
	{
		Tuple.Create("hair_GEO", ""),
		Tuple.Create("head_GEO", ""),
		Tuple.Create("mouth_GEO", ""),
		Tuple.Create("l_eye_GEO", ""),
		Tuple.Create("r_eye_GEO", ""),
		Tuple.Create("l_eyelash_GEO", ""),
		Tuple.Create("r_eyelash_GEO", ""),
	};
};

class Tuple<T,U>
{
	public T Item1 { get; private set; }
	public U Item2 { get; private set; }

	public Tuple(T item1, U item2)
	{
		Item1 = item1;
		Item2 = item2;
	}
}

static class Tuple
{
	public static Tuple<T, U> Create<T, U>(T item1, U item2)
	{
		return new Tuple<T, U>(item1, item2);
	}
}