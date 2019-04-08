package com.am.images;

import java.awt.image.BufferedImage;

/**
 * 图片验证码信息
 * 
 * @author am
 *
 */
public class VerificationCode {
	private String code = "";
	private BufferedImage image;

	/**
	 * 构造
	 */
	public VerificationCode() {
	}

	/**
	 * 构造
	 * @param code code
	 * @param image 图片信息
	 */
	public VerificationCode(String code, BufferedImage image) {
		this.setCode(code);
		this.setImage(image);
	}

	/**
	 * 图片Code
	 * 
	 * @return code
	 */
	public String getCode() {
		return code;
	}

	/**
	 * 图片Code
	 * 
	 * @param code
	 *            code
	 */
	public void setCode(String code) {
		this.code = code;
	}

	/**
	 * 图片信息
	 * 
	 * @return 图片信息
	 */
	public BufferedImage getImage() {
		return image;
	}

	/**
	 * 图片信息
	 * 
	 * @param image
	 *            图片信息
	 */
	public void setImage(BufferedImage image) {
		this.image = image;
	}

}
