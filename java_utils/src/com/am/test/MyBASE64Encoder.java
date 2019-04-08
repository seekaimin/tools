package com.am.test;

import sun.misc.BASE64Encoder;

public class MyBASE64Encoder extends BASE64Encoder {
    @Override
    protected int bytesPerLine() {
        return 4096;
    }
}