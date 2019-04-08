package com.am.images;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.Random;

import javax.imageio.ImageIO;

import com.am.utilities.tools;

/**
 * 验证码生成帮助类
 * 
 * @author am
 *
 */
public class VerificationCodeHelper {

	/**
	 * 构造
	 */
	public VerificationCodeHelper() {

	}

	protected Random random = new Random();

	/**
	 * 随机缓冲区
	 */
	private String randomBuffer = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	/**
	 * 图片宽
	 */
	private int width = 100;
	/**
	 * 图片高
	 */
	private int height = 40;
	/**
	 * 干扰线数量
	 */
	private int lineCount = 40;
	/**
	 * 干扰线长度
	 */
	private int linesize = 60;
	/**
	 * 随机产生字符数量
	 */
	private int codeCount = 4;

	private int fontSize = 20;

	/*
	 * 获得字体
	 */
	private Font getFont() {
		String[] fontName = new String[] { "KaiTi", "Fixedsys", "FangSong", "Microsoft YaHei" };
		int num = random.nextInt(fontName.length);
		return new Font(fontName[num], Font.CENTER_BASELINE, this.getFontSize());
		// 获取系统中可用的字体的名字
		// GraphicsEnvironment e =
		// GraphicsEnvironment.getLocalGraphicsEnvironment();
		// String[] fontName = e.getAvailableFontFamilyNames();
		// int num = random.nextInt(fontName.length);
		// for(int i = 0; i<fontName.length ; i++)
		// {
		// System.out.println(fontName[i]);
		// }
		// return new Font(fontName[num], Font.CENTER_BASELINE, fontSize);
		/*
		 * return (num % 2 == 0) ? new Font("Fixedsys", Font.CENTER_BASELINE,
		 * fontSize) : new Font("Times New Roman", Font.CENTER_BASELINE,
		 * fontSize);
		 */
	}

	/*
	 * 获得颜色
	 */
	private Color getLineColor(int fc, int bc) {
		if (fc > 255)
			fc = 255;
		if (bc > 255)
			bc = 255;
		int r = fc + random.nextInt(bc - fc - 16);
		int g = fc + random.nextInt(bc - fc - 14);
		int b = fc + random.nextInt(bc - fc - 18);
		return new Color(r, g, b);
	}

	/*
	 * 获得颜色
	 */
	private Color getRandomColor() {
		int r = random.nextInt(101);
		int g = random.nextInt(111);
		int b = random.nextInt(121);
		Color color = new Color(r, g, b);
		return color;
	}

	/**
	 * 获取验证码字符串
	 * 
	 * @return
	 */
	private String getRandomString() {
		StringBuffer result = new StringBuffer();
		for (int i = 0; i < this.getCodeCount(); i++) {
			int num = random.nextInt(this.getRandomBuffer().length());
			String temp = String.valueOf(this.getRandomBuffer().charAt(num));
			result.append(temp);
		}
		return result.toString();
	}

	/**
	 * 获取验证码信息
	 * 
	 * @return 验证码信息
	 */
	public VerificationCode getVerificationCode() {
		VerificationCode result = null;
		// BufferedImage类是具有缓冲区的Image类,Image类是用于描述图像信息的类
		BufferedImage image = new BufferedImage(this.getWidth(), this.getHeight(), BufferedImage.TYPE_INT_BGR);
		try {
			this.drawBackground(image, Color.white);
			// 绘制干扰线
			this.drawLine(image);
			// 绘制随机字符
			String code = this.getRandomString();
			this.drowString(image, code);
			result = new VerificationCode(code, image);
		} catch (Exception e) {
			throw e;
		} finally {
			image.getGraphics().dispose();
		}
		return result;
	}

	/**
	 * 绘制背景
	 * 
	 * @param image
	 * @param bg
	 */
	private void drawBackground(BufferedImage image, Color bg) {
		Graphics g = image.getGraphics();
		g.setColor(bg);
		g.fillRect(0, 0, this.getWidth(), this.getHeight());
	}

	/**
	 * 绘制干扰线
	 * 
	 * @param g
	 */
	private void drawLine(BufferedImage image) {
		Graphics g = image.getGraphics();
		g.setFont(new Font("Times New Roman", Font.ROMAN_BASELINE, 20));
		Color lineColor = getLineColor(100, 140);
		g.setColor(lineColor);
		for (int i = 0; i < this.getLineCount(); i++) {
			int x = random.nextInt(image.getWidth());
			int y = random.nextInt(image.getHeight());
			int xl = random.nextInt(this.linesize);
			int yl = random.nextInt(this.linesize);
			g.drawLine(x, y, x + xl, y + yl);
		}
	}

	/**
	 * 绘制字符串
	 * 
	 * @param image
	 * @param src
	 *            需要绘制的字符串
	 */
	private void drowString(BufferedImage image, String src) {
		Graphics g = image.getGraphics();
		int left = (this.getWidth() - this.getFontSize() * src.length()) / 2;
		int top = (this.getFontSize() + this.getHeight()) / 2;
		if (left < 0) {
			left = 0;
		}
		int pos = left;
		for (int i = 0; i < src.length(); i++) {
			char temp = src.charAt(i);
			g.setFont(getFont());
			Color color = this.getRandomColor();
			g.setColor(color);
			int step = random.nextInt(2);
			int flag = random.nextInt(1);
			step = flag == 0 ? step : step * -1;
			g.translate(step, 0);
			g.drawString(temp + "", pos, top);
			pos += this.getFontSize();
			//if (temp > 'z') {
			//} else {
			//	pos += ((this.getFontSize() * 1) / 2);
			//}
		}
	}

	/**
	 * 保存图片
	 * 
	 * @param iamge
	 * @param path
	 */
	public void savePic(BufferedImage iamge, String path) {

		// 再创建一个Graphics变量，用来画出来要保持的图片，及上面传递过来的Image变量
		Graphics g = iamge.getGraphics();
		try {
			g.drawImage(iamge, 0, 0, null);
			// 将BufferedImage变量写入文件中。
			ImageIO.write(iamge, "jpg", new File(path));
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public static void main(String args[]) {

		tools.println("0:%s",(int)'0');
		tools.println("9:%s",(int)'9');
		tools.println("Z:%s",(int)'Z');
		tools.println("z:%s",(int)'z');
		tools.println("我:%s",(int)'我');
		
		VerificationCodeHelper a = new VerificationCodeHelper();
		a.setCodeCount(6);
		a.setWidth(100);
		a.setHeight(30);
		for (int i = 0; i < 0; i++) {
			VerificationCode img = a.getVerificationCode();
			tools.println(img.getCode());
			tools.sleep(100);
		}
		// a.savePic(img.getValue(), "d:/eglogs/gray11.jpg");
	}

	public String getRandomBuffer() {
		return randomBuffer;
	}

	public void setRandomBuffer(String randomBuffer) {
		this.randomBuffer = randomBuffer;
	}

	public int getWidth() {
		return width;
	}

	public void setWidth(int width) {
		this.width = width;
	}

	public int getHeight() {
		return height;
	}

	public void setHeight(int height) {
		this.height = height;
	}

	public int getLineCount() {
		return lineCount;
	}

	public void setLineCount(int lineCount) {
		this.lineCount = lineCount;
	}

	public int getCodeCount() {
		return codeCount;
	}

	public void setCodeCount(int codeCount) {
		if (codeCount < 1) {
			codeCount = 1;
		}
		this.codeCount = codeCount;
	}

	public int getFontSize() {
		return fontSize;
	}

	public void setFontSize(int fontSize) {
		this.fontSize = fontSize;
	}

}