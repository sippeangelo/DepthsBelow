luanet.load_assembly("Microsoft.Xna.Framework")
Color = luanet.import_type("Microsoft.Xna.Framework.Color")

local frame = CreateFrame('Frame');
frame.X = 100;

local button = CreateFrame('Button', frame); 
button:SetTexture('images/Enter'); 
button.Width = 100; 
button.Height = 50; 
button.OnClick = function(clickPos)
	Console.WriteLine(clickPos.X .. ' ' .. clickPos.Y)
end


function CreatePanicFrame(x, y)
	local frame = CreateFrame("Frame")
	frame:SetTexture("images/GUI/unit_background")
	frame.Width = 154
	frame.Height = 19
	frame.X = x
	frame.Y = y
	
	local healthBg = CreateFrame("Frame", frame)
	healthBg:SetTexture("images/GUI/health_background")
	healthBg.Width = 136
	healthBg.Height = 13
	healthBg.X = 3
	healthBg.Y = 3
	
	local healthBar = CreateFrame("Frame", healthBg)
	healthBar:SetTexture("images/GUI/bar_solid")
	healthBar.Color = Color(87, 55, 253)
	healthBar.Width = healthBg.Width * 0.5
	healthBar.Height = 11
	healthBar.X = 1
	healthBar.Y = 1
	
	return frame
end

function CreateUnitFrame(name, x, y)
	-- Frames for one unit
	local frame = CreateFrame("Frame")
	frame:SetTexture("images/GUI/unit_background")
	frame.Width = 164;
	frame.Height = 35;
	frame.X = x
	frame.Y = y
	
	local nameText = CreateFrame("Text", frame)
	nameText:SetFont("fonts/UnitName")
	nameText.Value = name
	nameText.X = 4
	nameText.Y = 1

	local healthBg = CreateFrame("Frame", frame)
	healthBg:SetTexture("images/GUI/health_background")
	healthBg.Width = 136
	healthBg.Height = 13
	healthBg.X = 3
	healthBg.Y = 19
	
	local healthBar = CreateFrame("Frame", healthBg)
	healthBar:SetTexture("images/GUI/bar_solid")
	healthBar.Color = Color.Red
	healthBar.Width = healthBg.Width * 0.5
	healthBar.Height = 11
	healthBar.X = 1
	healthBar.Y = 1
	
	return frame
end

--[[local frame;
frame = CreatePanicFrame(0, 0)
frame = CreateUnitFrame("Flight captain Rainbow Dash", frame.X, frame.Y + frame.Height)
CreateUnitFrame("Mr. Sparkle", frame.X, frame.Y + frame.Height)]]--

local testFrame = CreateTestFrame()
testFrame:SetWidth(1337)