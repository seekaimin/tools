package com.am.test;

public class LogicNumberState implements Comparable<LogicNumberState> {
	String logicnumber = "";
	int power = 0;

	public LogicNumberState() {
	}

	public LogicNumberState(String code, int p) {
		setLogicnumber(code);
		setPower(p);
	}

	public String getLogicnumber() {
		return logicnumber;
	}

	public void setLogicnumber(String logicnumber) {
		this.logicnumber = logicnumber;
	}

	public int getPower() {
		return power;
	}

	public void setPower(int power) {
		this.power = power;
	}

	@Override
	public String toString() {
		return String.format("%s----%d", getLogicnumber(), getPower());
	}

	/**
	 * 传入的大 ： 1 <br />
	 * 传入的小 ： -1 <br />
	 * 一样大 ： 0
	 */
	@Override
	public int compareTo(LogicNumberState o) {
		int result = 0;
		int osize = o.getLogicnumber().length();
		int tsize = getLogicnumber().length();
		int maxength = osize > tsize ? tsize : osize;

		int i = 0;
		while (i < maxength) {
			char oc = o.getLogicnumber().charAt(i);
			char tc = getLogicnumber().charAt(i);
			if (oc > tc) {
				result = 1;
				break;
			} else if (oc < tc) {
				result = -1;
				break;
			}
			i++;
		}
		if (result == 0 && osize != tsize) {
			result = osize > tsize ? 1 : -1;
		}
		return result;
	}

	/**
	 * -1 : 无关 <br />
	 * 0 : 相等 <br />
	 * 1 ： 传入的大 <br />
	 * 2 ： 传入的小
	 */
	public int compare(String code) {
		int result = -1;	
		if (code.equals(getLogicnumber())) {
			result = 0;
		} else if (getLogicnumber().startsWith(code)) {
			result = 1;
		} else if (code.startsWith(getLogicnumber())) {
			result = 2;
		}
		/*
		if (code.equals(getLogicnumber())) {
			return 0;
		}
		int osize = code.length();
		int tsize = getLogicnumber().length();
		int maxength = osize > tsize ? tsize : osize;
		result = osize > tsize ? 2 : 1;
		int i = 0;
		while (i < maxength) {
			char oc = code.charAt(i);
			char tc = getLogicnumber().charAt(i);
			if (oc != tc) {
				result = -1;
				break;
			}
			i++;
		}
		*/
		return result;
	}
}
