package ${testClass.Package};

import java.io.IOException;
import net.sf.jni4net.Bridge;
import osmo.tester.annotation.AfterTest;
import osmo.tester.annotation.BeforeTest;
import osmo.tester.annotation.Guard;
import osmo.tester.annotation.TestStep;
import osmo.tester.annotation.Variable;
import ${testClass.ModelNamespace}.${testClass.ModelType};

public class ${testClass.Name} {
	
	@Variable
	private ${testClass.ModelType} model;

	static {
        try {
			Bridge.init();
	        Bridge.LoadAndRegisterAssemblyFrom(new java.io.File("lib/${testClass.ModelAssembly}.j4n.dll"));
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public ${testClass.Name}() {

	}

    #foreach($testStep in $testClass.TestSteps)
	  #if ($testStep.GuardAvailable)
	@Guard("${testStep.Name}")
	public boolean ${testStep.GuardMethodName}() {
		return ${testClass.ModelType}.${testStep.Guard}(${testStep.Parameter});
	}
	  #end
	  #if ($testStep.IsBeforeTest)
    @BeforeTest
	  #else
	    #if ($testStep.IsAfterTest)
    @AfterTest
	    #else
	@TestStep("${testStep.Name}")
	    #end
	  #end
	public void ${testStep.MethodName}() {
		${testClass.ModelType}.${testStep.Method}(${testStep.Parameter});
	}

	#end
}
