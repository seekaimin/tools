package com.am.services.nio.event;

import com.am.services.nio.NioRequest;
import com.am.services.nio.NioResponse;

/**
 * <p>Title: 服务器事件监听器</p>
 * @author starboy
 */

public interface NioServerListener {

   /**
    * 服务器端处理产生错误时触发本事件
    * @param e 错误信息
    */
   public void onError(Exception e);

   /**
    * 当有客户端发来请求时触发本事件
    */
   public void onAccept() throws Exception;

   /**
    * 当服务端接受客户端请求后触发本事件
    * @param request 客户端请求
    */
   public void onAccepted(NioRequest request) throws Exception;

   /**
    * 当客户端发来数据，并已被服务器控制线程正确读取时，触发该事件
    * @param request 客户端请求
    */
   public void onRead(NioRequest request) throws Exception;

   /**
    * 当可以向客户端发送请求触发本事件
    * @param request 客户端请求
    */
   public void onWrite(NioRequest request, NioResponse response) throws Exception;

   /**
    * 当客户端与服务器结束连接后触发本事件
    * @param request 客户端请求
    */
   public void onClosed(NioRequest request);
}
