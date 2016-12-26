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
<xsl:variable name="ourTeamPromos" select="umbraco.library:GetXmlNodeById($currentPage/ourTeamPromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->
	
	<section id="our_team">
		<div class="container">

			<xsl:for-each select="$ourTeamPromos/descendant::*[@isDoc]">

				<xsl:choose>
					<xsl:when test="position() = 1">

						<div class="row">
							<div class="col-sm-6">
								<h3>
									<xsl:value-of select="./title"/>
								</h3>
								<xsl:value-of select="./description" disable-output-escaping="yes"/>
							</div>
							<div class="col-sm-6 text-center">
								<xsl:variable name="mediaId" select="./image" />
								<xsl:if test="$mediaId != ''">
									<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"				
										 class="img-responsive out_team_img_tab team_mainimg"
										 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
										 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
									</img>
								</xsl:if>
							</div>
						</div>
						<div class="our_team_line">&nbsp;</div>
					</xsl:when>
					<xsl:otherwise>
						
						<xsl:choose>
							<xsl:when test="position() mod 2 = 0">
								<div >
									
									<xsl:choose>
										<xsl:when test="position() = 2">
											<xsl:attribute name="class">
												 	<xsl:value-of select="'row our_team_BlueChalk_section'" />
												</xsl:attribute>
										</xsl:when>
										<xsl:otherwise>
											<xsl:attribute name="class">
												 	<xsl:value-of select="'row our_team_Floral_White_section'" />
												</xsl:attribute>
										</xsl:otherwise>
									</xsl:choose>

									

									<div class="col-sm-10 our_team_BlueChalk_bg">
										<h4><xsl:value-of select="./title"/></h4>
										<xsl:value-of select="./description" disable-output-escaping="yes"/>
									</div>
									<div class="col-sm-2 text-center our_team_BlueChalk_bg_img ">
										<xsl:variable name="mediaId" select="./image" />
								<xsl:if test="$mediaId != ''">
									<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"				
										 class="img-responsive out_team_img_tab"
										 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
										 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
									</img>
								</xsl:if>
									</div>
								</div>
							</xsl:when>
							<xsl:otherwise>
								<div class="row our_team_Floral_White_section">
									<div class="col-sm-2 text-center our_team_BlueChalk_bg_img_right erl_ourTm ">
										<xsl:variable name="mediaId" select="./image" />
								<xsl:if test="$mediaId != ''">
									<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"				
										 class="img-responsive out_team_img_tab "
										 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
										 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
									</img>
								</xsl:if>
									</div>
									<div class="col-sm-10 our_team_Floral_White_bg">
										<h4><xsl:value-of select="./title"/></h4>
										<xsl:value-of select="./description" disable-output-escaping="yes"/>
									</div>
								</div>


                <div class="row ltr_ourTm">
                  <div class="col-sm-10 ">&nbsp;</div>
                  <div class="col-sm-2 text-center our_team_BlueChalk_bg_img_right ltr_ourTm">
                    <xsl:variable name="mediaId" select="./image" />
                    <xsl:if test="$mediaId != ''">
                      <img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
                         title="{$currentPage/@nodeName}"
                         alt="{$currentPage/@nodeName}"
                         class="img-responsive out_team_img_tab "
                         width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
                         height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
                      </img>
                    </xsl:if>                    
                  </div>
                </div>
							</xsl:otherwise>
						</xsl:choose>	
						
						<xsl:if test="position() = last()">
							<div class="our_team_line">&nbsp;</div>
						</xsl:if>
	  
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</div>



	</section>
	 

</xsl:template>

</xsl:stylesheet>