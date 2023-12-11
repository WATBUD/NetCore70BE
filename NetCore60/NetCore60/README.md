# ---------String interpolation------------------------
# JS using `${var}`
`string text ${expression} string text`
# C# using $"{var}"
// Composite formatting:
Console.WriteLine("Hello, {0}! Today is {1}, it's {2:HH:mm} now.", name, date.DayOfWeek, date);
// String interpolation:
Console.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");


# `���Ѷ}�L�ɶ��O�P���@�ܬP�����W��9�G30�ܤU��16�G00�I�A���L����ĮL�O�ɶ��M�V�O�ɶ��A������x�W���ɶ��|���Ҥ��P�G
#  �L�O�ɶ��]3�뤤-11�뤤�^�G�x�W�ɶ�21�G30~04�G00
#  �V�O�ɶ��]11�뤤-3�뤤�^�G�x�W�ɶ�22�G30~05�G00
#  let corporationData = `  �x�W�Ҩ����ҷ|�b�U��4�I������Ѫk�H�R��W���`;

# �@�B�x�ѤU�����AROD �B IOC �B FOK ���ƻ�t�O�H
# (1) ROD :��馳��(Rest of Day)
# �`�����w�]����覡�A�Q�n���������즬�L�����ġA�N�ϥ�ROD�C�@��ӻ��A���䪺�B�ͥ���̱`�Ψ즹����O��
# (2) IOC�G�ߧY����_�h����(Immediate-or-Cancel)
# ���\��������A�ӨS�����檺�����N�����C�q�`�O��������O�|�f�tIOC�C
# (3) FOK�G��������_�h����(Fill-or-Kill)
# �@�w�n��������A�_�h�N���������C