# ---------String interpolation------------------------
# JS using `${var}`
`string text ${expression} string text`
# C# using $"{var}"
// Composite formatting:
Console.WriteLine("Hello, {0}! Today is {1}, it's {2:HH:mm} now.", name, date.DayOfWeek, date);
// String interpolation:
Console.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");


# `美股開盤時間是星期一至星期五上午9：30至下午16：00點，不過美國採夏令時間和冬令時間，對應到台灣的時間會有所不同：
#  夏令時間（3月中-11月中）：台灣時間21：30~04：00
#  冬令時間（11月中-3月中）：台灣時間22：30~05：00
#  let corporationData = `  台灣證券交易所會在下午4點公布當天法人買賣超資料`;