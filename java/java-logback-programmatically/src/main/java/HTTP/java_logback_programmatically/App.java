package HTTP.java_logback_programmatically;

import org.slf4j.LoggerFactory;
import ch.qos.logback.classic.Logger;
import ch.qos.logback.classic.LoggerContext;
import ch.qos.logback.classic.encoder.PatternLayoutEncoder;
import ch.qos.logback.classic.spi.ILoggingEvent;
import ch.qos.logback.core.ConsoleAppender;
import ch.qos.logback.ext.loggly.LogglyAppender;

/**
 * Hello world!
 *
 */
public class App 
{
	final static Logger rootLogger = (Logger)LoggerFactory.getLogger(App.class);
	static public void main(String[] args) throws Exception {  
		generateError ge = new generateError();
	    LoggerContext loggerContext = rootLogger.getLoggerContext();

	    PatternLayoutEncoder encoder = new PatternLayoutEncoder();
	    encoder.setContext(loggerContext);
	    encoder.setPattern("%d{\\\"ISO8601\\\", UTC}  %p %t %c %M - %m%n");
	    encoder.start();

	    ConsoleAppender<ILoggingEvent> appender = new ConsoleAppender<ILoggingEvent>();
	    appender.setContext(loggerContext);
	    appender.setEncoder(encoder); 
	    appender.start();
	    
	    LogglyAppender<ILoggingEvent> logapp = new LogglyAppender<ILoggingEvent>();
	    logapp.setContext(loggerContext);
	    logapp.setName("loggly");
	    logapp.setPattern("%d{\\\"ISO8601\\\", UTC}  %p %t %c %M - %m%n");
	    logapp.setEndpointUrl(String.format("http://logs-01.loggly.com/inputs/CUSTOMER-TOKEN/tag/logback-working"));
	    logapp.start();

	    rootLogger.addAppender(appender);
	    rootLogger.addAppender(logapp);

	    rootLogger.debug("Message 1"); 
	    rootLogger.warn("Message 2");
	    rootLogger.info("Hey there..");
	    ge.displayMessage();
	}
}

class generateError {
	void displayMessage() {
		try {
			int data = 50 / 0;
		} catch (ArithmeticException e) {
			System.out.println(e);
			App.rootLogger.info(e.toString());
			App.rootLogger.info("I'm from different class");
		}
		System.out.println("Print Message here for exception...");
	}
}