ToolTip = CreateFrame("Frame")

local text = CreateFrame("Text", ToolTip)
text:SetFont("Arial")
text.Value = "99%"
ToolTip.Text = text
