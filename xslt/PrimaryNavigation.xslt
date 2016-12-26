<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
	version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library" xmlns:Examine="urn:Examine" 
	exclude-result-prefixes="msxml umbraco.library Examine ">


<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>
	
<xsl:variable name="rootnode" select="$currentPage/ancestor-or-self::*[@level = 1]/@id"/>
<xsl:variable name="level" select="2"/>
<xsl:variable name="level3" select="3"/>
	
<xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'" />
<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />

<xsl:template match="/">

<!-- start writing XSLT -->
	
	<nav class="col-md-8 col-sm-8 col-xs-8"> <a class="cmn-toggle-switch cmn-toggle-switch__rot  open_close" href="javascript:void(0);"><span>Menu mobile</span></a>
		<div class="main-menu">
			<div id="header_menu"> <img src="images/Logo_mobile.png" alt="CountryHolidays" data-retina="true" /> </div>
				<a href="#" class="open_close" id="close_in"><i class=" fa fa-close">&nbsp;</i></a>
				<ul>
					<li > <a href="#Tell-us-aboutyourself">HOW DOES IT WORK </a> </li>
					
					<xsl:for-each select="$currentPage/ancestor-or-self::*//* [@isDoc and @level=$level and string(pageHide) != '1']">
						<xsl:if test="./parent::*[@isDoc]/@id = $rootnode">
						<li >
							<a href="{umbraco.library:NiceUrl(./@id)}">										
								<xsl:value-of select="translate(./@nodeName, $smallcase, $uppercase)"/> 		
								
							</a>
						</li>
							</xsl:if>
					</xsl:for-each>
					
					<li class="submenu"> <a href="javascript:void(0);" class="show-submenu">LOGIN</a>
						<ul>
						<xsl:for-each select="$currentPage/ancestor-or-self::*//* [@isDoc and @level=$level3 and string(pageHide) != '1']">
						<xsl:if test="./parent::*[@isDoc]/parent::*[@isDoc]/@id = $rootnode">
							<xsl:choose>
								<xsl:when test="./@nodeName = 'Login' ">
									<span>Already an account?</span>
								</xsl:when>
								<xsl:otherwise>
									 <span>No account yet?</span>
									
								</xsl:otherwise>
							</xsl:choose>

						<li >
							<a href="{umbraco.library:NiceUrl(./@id)}">										
								<xsl:value-of select="./@nodeName"/>		
							</a>
						</li>
							</xsl:if>
					</xsl:for-each>
                
                
                  </ul>
                
              </li>

					
              <li class="submenu"><a href="#">| NL<span class="down_arrow"> <i class="fa fa-angle-down ">&nbsp;</i></span></a> 
                <ul>
             
                  <li><a href="Your_scotty_bons_account2_Nomenu.html">NL - en</a></li>
        
                  <li><a href="Your_scotty_bons_account2_Nomenu.html"> NL - nl</a></li>
                </ul>
              </li>
					
					
					
					
				</ul>
		</div>
		
		<!-- End main-menu -->
	</nav>

</xsl:template>

</xsl:stylesheet>