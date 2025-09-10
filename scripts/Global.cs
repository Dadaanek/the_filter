using Godot;
using System;
using System.Collections.Generic;

#nullable enable

public static class Global
{
	// 	// used so that tiles and sprites are animated from the same point in time
	// 	public static void StartAnimation(AnimatedSprite2D animatedSprite, float animationSpeed, int framesCount)
	// 	{
	// 		float timePerFrame = 1f / animationSpeed * framesCount;
	// 		float timeElapsed = (float) ((float) Time.GetTicksUsec() / 1e6);
	// 		float frameTimeOffset = timeElapsed % timePerFrame;

	// 		GD.Print(frameTimeOffset);

	// 		animatedSprite.Play();
	// 		animatedSprite.SetFrameAndProgress( 0, frameTimeOffset);
	// 	}

	public static void CustomizeLabel(RichTextLabel popupLabel, int? width, Vector2? size,
	Color? bgColor, int? margins, int? labelSize, string? fontSource)
	{
		if (size != null)
			popupLabel.Size = (Vector2) size;

		popupLabel.FitContent = true;
		popupLabel.AutowrapMode = TextServer.AutowrapMode.Word;

		var style = new StyleBoxFlat();
		// transparent background
		style.BgColor = new Color(1, 1, 1, 0);

		if (bgColor != null)
			style.BgColor = (Color)bgColor;

		if (margins != null)
		{
			style.ContentMarginLeft = (int) margins;
			style.ContentMarginTop = (int) margins;
			style.ContentMarginRight = (int) margins;
			style.ContentMarginBottom = (int) margins;
		}

		popupLabel.AddThemeStyleboxOverride("normal", style);

		// setting font
		if (labelSize != null)
			popupLabel.AddThemeFontSizeOverride("normal_font_size", (int) labelSize);

		if (fontSource != null)
		{
			var fontFile = ResourceLoader.Load<FontFile>((string) fontSource);
			popupLabel.AddThemeFontOverride("normal_font", fontFile);
		}
	}
}
