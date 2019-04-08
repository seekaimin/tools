package com.am.servlets;

import java.io.File;
import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.am.io.WebUpLoadItem;
import com.am.io.fileuploadhelpers;

/**
 * Servlet implementation class scServlet
 */
@WebServlet("/scServlet")
public class scServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;

	/**
	 * @see HttpServlet#HttpServlet()
	 */
	public scServlet() {
		super();
		// TODO Auto-generated constructor stub
	}

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse
	 *      response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		// TODO Auto-generated method stub
		response.getWriter().append("Served at: ").append(request.getContextPath());
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse
	 *      response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		String savePath = this.getServletContext().getRealPath("/upload");
		WebUpLoadItem data = new WebUpLoadItem();
		boolean flag = fileuploadhelpers.doUpLoad(savePath, request, data);
		int flagval = flag ? 0 : 1;
		String serverid = data.getFileNewFullName().replace( File.separator, "#");
		String d = "{\"flag\":" + flagval + ",\"fileserverid\":\"" + serverid + "\"}";
		response.getWriter().write(d);
	}

}
