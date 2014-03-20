//
//  DMOAnalitics.m
//  Unity-iPhone
//
//  Created by Juan Cucurella on 10/18/13.
//
//

#import <UIKit/UIKit.h>
#import "UnityAppController.h"
#import "ExtendedUnityAppController.h"
#import "AnalyticsManager.h"
#import "CSComScore.h"


#define IMPL_APP_CONTROLLER_SUBCLASS(ExtendedUnityAppController)
@interface ExtendedUnityAppController(OverrideAppDelegate)
{
}
+(void)load;
@end
@implementation ExtendedUnityAppController(OverrideAppDelegate)
+(void)load
{
    extern const char* AppControllerClassName;
    AppControllerClassName = "ExtendedUnityAppController";
}
@end

//	Crittercism Call into library for init
const char* CRITTERCISM_APPKEY	= "52614de48b2e337efe000004";


@implementation ExtendedUnityAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    application.applicationIconBadgeNumber = 0;
    
    //DMO analitics
    [[AnalyticsManager sharedAnalyticsManger] startAnalyticsWithAppID:DMO_APPKEY appSecret:DMO_SECRETKEY];
    [[AnalyticsManager sharedAnalyticsManger] logEventUserInfoWithDeviceId:nil userId:nil userProfile:nil];
    [[AnalyticsManager sharedAnalyticsManger] logEventPlayerInfoWithDeviceId:nil playerId:nil];
    
    [[AnalyticsManager sharedAnalyticsManger] startCrittercismWithAppID:CRITTERCISM_APPKEY];
    
    NSLog(@"init custom appcontroller for analytics");
    //[[AnalyticsManager sharedAnalyticsManger] logEventWithName:@"app_start"];
    
    [CSComScore setAppContext];
    [CSComScore setCustomerC2:@"6035140"];
    [CSComScore setPublisherSecret:@"bacd860dcd22dd180bdcb7c680f64060"];
    [CSComScore setAppName:@"Frozen Free Fall"];
    
    //	Initialize Crittercism so we can see unity startup crashes
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}


- (void)application:(UIApplication *)application didReceiveLocalNotification:(UILocalNotification *)notification
{
    // Set icon badge number to zero
    application.applicationIconBadgeNumber = 0;
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
    //[[AnalyticsManager sharedAnalyticsManger] logEventWithName:@"app_foreground"];
    [super applicationWillEnterForeground:application];
}

- (void)applicationDidEnterBackground:(UIApplication *)application {
    //[[AnalyticsManager sharedAnalyticsManger] logEventWithName:@"app_background"];
    
    application.applicationIconBadgeNumber = 0;
    
    [super applicationDidEnterBackground:application];
}

- (void)applicationWillTerminate:(UIApplication *)application {
    //[[AnalyticsManager sharedAnalyticsManger] logEventWithName:@"app_end"];
    [super applicationWillTerminate:application];
}


@end

