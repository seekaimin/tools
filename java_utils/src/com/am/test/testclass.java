package com.am.test;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class testclass {
	public static void a(String code)
	{
		List<LogicNumberState> list = new ArrayList<LogicNumberState>();
		list.add(new LogicNumberState("101010", 0));
		list.add(new LogicNumberState("101011", 1));
		list.add(new LogicNumberState("1010", 1));
		list.add(new LogicNumberState("1010101010", 0));
		list.add(new LogicNumberState("101013", 1));
		list.add(new LogicNumberState("10101010", 1));
		list.add(new LogicNumberState("101012", 1));

		LogicNumberState pre = new LogicNumberState("", 0);
		Collections.sort(list, Collections.reverseOrder());
		for (LogicNumberState a : list) {

			System.out.println(a);
		}
		System.out.println("------------------------------------------");
		for (LogicNumberState a : list) {
			System.out.println("item:" + a);
			int f = a.compare(code);
			System.out.println("compare:" + f);
			if (f == 0) {
				pre = a;
				break;
			} else if (f == 1) {
				System.out.println("code 大");
				pre = a;
				break;
			} else if(f == 2){
				pre = a;
				System.out.println("code 小");
				//break;
			}else
			{
				//if(!pre.getLogicnumber().equals(""))
				{
				//	break;
				}
			}
		}

		System.out.println("------------------------------------------");
		System.out.println("src:" + code);
		System.out.println("result:" + pre);
	}
	public static void b()
	{
		A a = new A();
		B b = new B();
		System.out.println(a.Show());
		System.out.println(b.Show());
		
	}
}
