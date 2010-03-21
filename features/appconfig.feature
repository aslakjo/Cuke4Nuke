Feature: Load config file for step definition DLL

  Background:
    Given a standard Cucumber project directory structure
    And a file named "features/appconfig.feature" with:
      """
        Scenario: Load config
          Then the config file should load

      """
    And a file named "features/step_definitions/some_remote_place.wire" with:
      """
      host: localhost
      port: 3901

      """
    And a file named "bin/GeneratedStepDefinitions.dll.config" with:
      """
      <?xml version="1.0" encoding="utf-8" ?>
	  <configuration>
	  <configSections>
			<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
		  </configSections>
		  
		  <appSettings>
			<add key="helloMessage" value="Hello Cuke4Nuke!" />
		  </appSettings>
		  
		  <log4net>
			<root>
			  <level value="ALL" />
			  <appender-ref ref="LogFileAppender" />
			</root>
			<appender name="LogFileAppender" type="log4net.Appender.RollingFileAppender,log4net">
			  <param name="File" value="Cuke4NukeLog.txt" />
			  <param name="AppendToFile" value="true" />
			  <rollingStyle value="Size" />
			  <maxSizeRollBackups value="2" />
			  <maximumFileSize value="100KB" />
			  <staticLogFileName value="true" />
			  <datePattern value="yyyyMMdd" />
			  <layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
			  </layout>
			</appender>
		  </log4net>
        
      </configuration>
      """

    Scenario: A passing step
      Given a step definition assembly containing:
        """
        public class GeneratedSteps
        {
          [Then("^the config file should load$")]
          public static void ConfigFileShouldLoad()
          {
            string expectedValue = "Hello Cuke4Nuke!";
            string actualValue = System.Configuration.ConfigurationManager.AppSettings["helloMessage"]; 
            if (!actualValue.Equals(expectedValue))
            {
              throw new Exception(
                String.Format(
                  "Config setting missing or incorrect. Expected <{0}>, got <{1}>.",
                  expectedValue,
                  actualValue
                )
              );
            }
          }
        }
        """
      When I run the cuke4nuke wrapper
      Then STDERR should be empty
      And it should pass with
        """
        .

        1 scenario (1 passed)
        1 step (1 passed)

        """