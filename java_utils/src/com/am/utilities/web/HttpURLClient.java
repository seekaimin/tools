package com.am.utilities.web;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.List;

import com.am.utilities.Encodings;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;
import com.am.utilities.settings.KeyValuePair;

/**
 * HttpURLClient 参数
 * 
 * @author Administrator
 *
 */
public class HttpURLClient {

	private String contentType = ContentTypes.applicationUrlencoded;
	private String url = null;
	private Methods method = Methods.GET;
	private int connectTimeout = 10000;
	private int readTimeout = 30000;
	private String cookie = null;
	private String encoding = Encodings.UTF8;
	private List<KeyValuePair<String, Object>> parameters = new ArrayList<KeyValuePair<String, Object>>();
	private boolean useCookie = false;
	private boolean stringData = false;

	public HttpURLClient(String url) {
		this.setUrl(url);
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public String getCookie() {
		return cookie;
	}

	public void setCookie(String cookie) {
		this.useCookie = true;
		this.cookie = cookie;
	}

	public boolean isUseCookie() {
		return this.useCookie;
	}

	public String getEncoding() {
		return encoding;
	}

	public void setEncoding(String encoding) {
		this.encoding = encoding;
	}

	public int getConnectTimeout() {
		return connectTimeout;
	}

	public void setConnectTimeout(int connectTimeout) {
		this.connectTimeout = connectTimeout;
	}

	public int getReadTimeout() {
		return readTimeout;
	}

	public void setReadTimeout(int readTimeout) {
		this.readTimeout = readTimeout;
	}

	public Methods getMethod() {
		return method;
	}

	public void setMethod(Methods method) {
		this.method = method;
	}

	public List<KeyValuePair<String, Object>> getParameters() {
		return parameters;
	}

	/**
	 * ContentTypes
	 * 
	 * @return
	 */
	public String getContentType() {
		return contentType;
	}

	/**
	 * ContentTypes
	 * 
	 * @param contentType
	 */
	public void setContentType(String contentType) {
		this.contentType = contentType;
	}

	/**
	 * 重置参数
	 */
	public void reset() {
		this.parameters.clear();
	}

	public void add(String key, Object... values) {
		if (values != null) {
			for (Object value : values) {
				KeyValuePair<String, Object> item = new KeyValuePair<String, Object>(key, value);
				this.parameters.add(item);
			}
		}
	}

	public void add(String values) {
		if (values == null) {
			values = "";
		}
		this.setStringData(true);
		this.parameters.clear();
		KeyValuePair<String, Object> item = new KeyValuePair<String, Object>("STRING", values);
		this.parameters.add(item);
	}

	public String getParameterPath() {
		StringBuffer result = new StringBuffer();
		if (this.isStringData()) {
			if (this.getParameters().size() > 0) {
				result.append(this.getParameters().get(0).getValue());
			}
		} else {
			for (int i = 0; i < this.parameters.size(); i++) {
				KeyValuePair<String, Object> item = this.getParameters().get(i);
				try {
					result.append(item.getKey()).append("=")
							.append(URLEncoder.encode(item.getValue().toString(), this.getEncoding())).append("&");
				} catch (Exception e) {
				}
			}
		}
		return result.toString();
	}

	private HttpURLConnection createHttpURLConnection() throws IOException {
		String url = this.getUrl();
		String args = this.getParameterPath();
		if (this.getMethod() == Methods.GET && (stringhelpers.isNullOrEmpty(args) == false)) {
			url = url + "?" + args;
		}
		URL realUrl = new URL(url);
		// 打开和URL之间的连接
		HttpURLConnection connection = (HttpURLConnection) realUrl.openConnection();
		connection.setConnectTimeout(this.getConnectTimeout());
		connection.setReadTimeout(this.getReadTimeout());
		// 设置通用的请求属性
		int contentlength = args.length();
		connection.setRequestMethod(this.getMethod().toString());
		if (this.getMethod() == Methods.POST) {
			connection.setRequestProperty("Accept", "application/json, text/javascript, */*; q=0.01");
			connection.setRequestProperty("Connection", "Keep-Alive");
			connection.setRequestProperty("Content-Length", contentlength + "");
			connection.setRequestProperty("Content-Type", this.getContentType() + ";charset=" + this.getEncoding());
		}
		connection.setRequestProperty("User-agent",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.89 Safari/537.36");
		connection.setRequestProperty("Accept-Language", "en,zh-CN;q=0.8,zh;q=0.6");
		// 建立实际的连接

		if (this.getMethod() == Methods.POST) {
			OutputStream out = null;
			try {
				connection.setDoOutput(true);
				connection.setDoInput(true);
				out = connection.getOutputStream();
				byte[] data = stringhelpers.toBytes(args, this.getEncoding());
				out.write(data, 0, data.length);
				out.flush();
			} catch (Exception ex) {
				throw ex;
			} finally {
				tools.close(out);
			}
		} else {
			// 建立实际的连接
			connection.connect();
		}
		return connection;
	}

	/**
	 * 请求发送
	 * 
	 * @return 返回字符串
	 * @throws IOException
	 */
	public String send() throws IOException {
		String result = "";
		BufferedReader in = null;
		// 打开和URL之间的连接
		try {
			HttpURLConnection connection = this.createHttpURLConnection();
			in = new BufferedReader(new InputStreamReader(connection.getInputStream(), this.getEncoding()));
			String line;
			while ((line = in.readLine()) != null) {
				result += line;
			}
		} catch (IOException e) {
			throw e;
		} finally {
			try {
				if (in != null) {
					in.close();
				}
			} catch (Exception e2) {
			}
		}

		return result;
	}

	public boolean isStringData() {
		return stringData;
	}

	public void setStringData(boolean stringData) {
		this.stringData = stringData;
	}

	public void send(final IHttpCall callback) {

		HttpURLConnection connection = null;
		BufferedReader in = null;
		// 打开和URL之间的连接
		try {
			connection = this.createHttpURLConnection();
			in = new BufferedReader(new InputStreamReader(connection.getInputStream(), this.getEncoding()));
			String line;
			String result = "";
			while ((line = in.readLine()) != null) {
				result += line;
			}
			callback.success(result);
		} catch (IOException e) {
			callback.error(e);
		} finally {
			try {
				tools.close(in);
				if (connection != null) {
					connection.disconnect();
				}
			} catch (Exception e2) {
			}
			callback.complete();
		}
	}
}
