package com.am.services.nio;

import java.util.ArrayList;

import com.am.services.nio.event.NioServerListener;

/**
 * <p>Title: 事件触发器</p>
 * @author starboy
 * @version 1.0
 */
public class NioNotifier {
    private ArrayList<NioServerListener> listeners = null;
    public NioNotifier() {
        listeners = new ArrayList<NioServerListener>();
    }
    /**
     * 添加事件监听器
     * @param l 监听器
     */
    public void addListener(NioServerListener l) {
        synchronized (listeners) {
            if (!listeners.contains(l))
                listeners.add(l);
        }
    }

    public void fireOnAccept() throws Exception {
        for (int i = listeners.size() - 1; i >= 0; i--)
            ( (NioServerListener) listeners.get(i)).onAccept();
    }

    public void fireOnAccepted(NioRequest request) throws Exception {
        for (int i = listeners.size() - 1; i >= 0; i--)
            ( (NioServerListener) listeners.get(i)).onAccepted(request);
    }

    void fireOnRead(NioRequest request) throws Exception {
        for (int i = listeners.size() - 1; i >= 0; i--)
            ( (NioServerListener) listeners.get(i)).onRead(request);

    }

    void fireOnWrite(NioRequest request, NioResponse response)  throws Exception  {
        for (int i = listeners.size() - 1; i >= 0; i--)
            ( (NioServerListener) listeners.get(i)).onWrite(request, response);

    }

    public void fireOnClosed(NioRequest request) {
        for (int i = listeners.size() - 1; i >= 0; i--)
            ( (NioServerListener) listeners.get(i)).onClosed(request);
    }

    public void fireOnError(Exception e) {
        for (int i = listeners.size() - 1; i >= 0; i--)
            ( (NioServerListener) listeners.get(i)).onError(e);
    }
}
