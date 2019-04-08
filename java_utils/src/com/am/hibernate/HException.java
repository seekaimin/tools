package com.am.hibernate;


public class HException extends Exception {
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 1384271137437048786L;
	private boolean commit = false;
	
	public HException()
	{
		
	}
	public HException(boolean iscommit)
	{
		this.setCommit(iscommit);
	}
	public boolean isCommit() {
		return commit;
	}

	public void setCommit(boolean commit) {
		this.commit = commit;
	}
	
}
