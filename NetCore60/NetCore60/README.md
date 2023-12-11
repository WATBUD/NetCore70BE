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

# 一、台股下單條件，ROD 、 IOC 、 FOK 有甚麼差別？
# (1) ROD :當日有效(Rest of Day)
# 常見的預設掛單方式，想要讓掛單持續到收盤都有效，就使用ROD。一般來說，身邊的朋友交易最常用到此條件呢～
# (2) IOC：立即成交否則取消(Immediate-or-Cancel)
# 允許部分成交，而沒有成交的部分就取消。通常是市價單指令會搭配IOC。
# (3) FOK：全部成交否則取消(Fill-or-Kill)
# 一定要全部成交，否則就全部取消。