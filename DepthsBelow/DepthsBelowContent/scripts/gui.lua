local frame = CreateFrame('Frame');
frame.X = 100;

local button = CreateFrame('Button', frame); 
button:SetTexture('images/Enter'); 
button.Width = 100; 
button.Height = 50; 
button.OnClick = function(clickPos)
	Console.WriteLine(clickPos.X .. ' ' .. clickPos.Y)
end

