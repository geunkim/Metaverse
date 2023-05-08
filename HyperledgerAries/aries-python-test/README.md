# aries-python-test

Hyperledger Aries Cloud Agent Python 코드 테스트를 위해 작성된 저장소

### 실행방법

윈도우 환경에서 실행하였으며 python 3.11.3 버전 사용

    $python --version
    Python 3.11.3

pip를 사용해 패키지 다운

    $pip --version
    pip 23.1

패키지의 경우 aries-python-test 폴더로 이동 후 pip로 패키지 다운

    $cd aries-python-test
    $pip install -r requirements.txt --user

python을 사용해 코드 실행

    $python main.py

### aries Protocol 정리

        protocols
        |   didcomm_prefix.py
        |
        +---actionmenu
        |   |   definition.py
        |   |
        |   \---v1_0
        |       |   base_service.py
        |       |   controller.py
        |       |   driver_service.py
        |       |   message_types.py
        |       |   routes.py
        |       |   util.py
        |       |
        |       +---handlers
        |       |       menu_handler.py
        |       |       menu_request_handler.py
        |       |       perform_handler.py
        |       |   
        |       |
        |       +---messages
        |       |       menu.py
        |       |       menu_request.py
        |       |       perform.py
        |       |   
        |       |
        |       \---models
        |               menu_form.py
        |               menu_form_param.py
        |               menu_option.py
        |       
        |
        +---basicmessage
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   message_types.py
        |       |   routes.py
        |       |
        |       +---handlers
        |       |       basicmessage_handler.py
        |       |   
        |       |
        |       \---messages
        |               basicmessage.py
        |          
        |       
        |
        +---connections
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   connection_invitation_handler.py
        |       |   |   connection_request_handler.py
        |       |   |   connection_response_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_invitation_handler.py
        |       |           test_request_handler.py
        |       |           test_response_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   connection_invitation.py
        |       |   |   connection_request.py
        |       |   |   connection_response.py
        |       |   |   problem_report.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_connection_invitation.py
        |       |           test_connection_request.py
        |       |           test_connection_response.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |       connection_detail.py
        |       |       __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---coordinate_mediation
        |   |   definition.py
        |   |   mediation_invite_store.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   controller.py
        |       |   manager.py
        |       |   message_types.py
        |       |   normalization.py
        |       |   routes.py
        |       |   route_manager.py
        |       |   route_manager_provider.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   keylist_handler.py
        |       |   |   keylist_query_handler.py
        |       |   |   keylist_update_handler.py
        |       |   |   keylist_update_response_handler.py
        |       |   |   mediation_deny_handler.py
        |       |   |   mediation_grant_handler.py
        |       |   |   mediation_request_handler.py
        |       |   |   problem_report_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_keylist_handler.py
        |       |           test_keylist_query_handler.py
        |       |           test_keylist_update_handler.py
        |       |           test_keylist_update_response_handler.py
        |       |           test_mediation_deny_handler.py
        |       |           test_mediation_grant_handler.py
        |       |           test_mediation_request_handler.py
        |       |           test_problem_report_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   keylist.py
        |       |   |   keylist_query.py
        |       |   |   keylist_update.py
        |       |   |   keylist_update_response.py
        |       |   |   mediate_deny.py
        |       |   |   mediate_grant.py
        |       |   |   mediate_request.py
        |       |   |   problem_report.py
        |       |   |   __init__.py
        |       |   |
        |       |   +---inner
        |       |   |       keylist_key.py
        |       |   |       keylist_query_paginate.py
        |       |   |       keylist_updated.py
        |       |   |       keylist_update_rule.py
        |       |   |       __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_keylist.py
        |       |           test_keylist_query.py
        |       |           test_keylist_update.py
        |       |           test_keylist_update_response.py
        |       |           test_mediate_deny.py
        |       |           test_mediate_grant.py
        |       |           test_mediate_request.py
        |       |           test_problem_report.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |       mediation_record.py
        |       |       __init__.py
        |       |
        |       \---tests
        |               test_mediation_invite_store.py
        |               test_mediation_manager.py
        |               test_routes.py
        |               test_route_manager.py
        |               __init__.py
        |
        +---didexchange
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   complete_handler.py
        |       |   |   invitation_handler.py
        |       |   |   request_handler.py
        |       |   |   response_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_complete_handler.py
        |       |           test_invitation_handler.py
        |       |           test_request_handler.py
        |       |           test_response_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   complete.py
        |       |   |   problem_report_reason.py
        |       |   |   request.py
        |       |   |   response.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_complete.py
        |       |           test_request.py
        |       |           test_response.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---discovery
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   +---v1_0
        |   |   |   manager.py
        |   |   |   message_types.py
        |   |   |   routes.py
        |   |   |   __init__.py
        |   |   |
        |   |   +---handlers
        |   |   |   |   disclose_handler.py
        |   |   |   |   query_handler.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_disclose_handler.py
        |   |   |           test_query_handler.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---messages
        |   |   |   |   disclose.py
        |   |   |   |   query.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_disclose.py
        |   |   |           test_query.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---models
        |   |   |   |   discovery_record.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_record.py
        |   |   |           __init__.py
        |   |   |
        |   |   \---tests
        |   |           test_manager.py
        |   |           test_routes.py
        |   |           __init__.py
        |   |
        |   \---v2_0
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   disclosures_handler.py
        |       |   |   queries_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_disclosures_handler.py
        |       |           test_queries_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   disclosures.py
        |       |   |   queries.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_disclosures.py
        |       |           test_queries.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |   |   discovery_record.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_record.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---endorse_transaction
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   controller.py
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   transaction_jobs.py
        |       |   util.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   endorsed_transaction_response_handler.py
        |       |   |   refused_transaction_response_handler.py
        |       |   |   transaction_acknowledgement_handler.py
        |       |   |   transaction_cancel_handler.py
        |       |   |   transaction_job_to_send_handler.py
        |       |   |   transaction_request_handler.py
        |       |   |   transaction_resend_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_endorsed_transaction_response_handler.py
        |       |           test_refused_transaction_response_handler.py
        |       |           test_transaction_acknowledgement_handler.py
        |       |           test_transaction_cancel_handler.py
        |       |           test_transaction_job_to_send_handler.py
        |       |           test_transaction_request_handler.py
        |       |           test_transaction_resend_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   cancel_transaction.py
        |       |   |   endorsed_transaction_response.py
        |       |   |   messages_attach.py
        |       |   |   refused_transaction_response.py
        |       |   |   transaction_acknowledgement.py
        |       |   |   transaction_job_to_send.py
        |       |   |   transaction_request.py
        |       |   |   transaction_resend.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_cancel_transaction.py
        |       |           test_endorsed_transaction_response.py
        |       |           test_messages_attach.py
        |       |           test_refused_transaction_response.py
        |       |           test_transaction_acknowledgement.py
        |       |           test_transaction_job_to_send.py
        |       |           test_transaction_request.py
        |       |           test_transaction_resend.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |       transaction_record.py
        |       |       __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---introduction
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v0_1
        |       |   base_service.py
        |       |   demo_service.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   forward_invitation_handler.py
        |       |   |   invitation_handler.py
        |       |   |   invitation_request_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_forward_invitation_handler.py
        |       |           test_invitation_handler.py
        |       |           test_invitation_request_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   forward_invitation.py
        |       |   |   invitation.py
        |       |   |   invitation_request.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_forward_invitation.py
        |       |           test_invitation.py
        |       |           test_invitation_request.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_routes.py
        |               test_service.py
        |               __init__.py
        |
        +---issue_credential
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   +---v1_0
        |   |   |   controller.py
        |   |   |   manager.py
        |   |   |   message_types.py
        |   |   |   routes.py
        |   |   |   __init__.py
        |   |   |
        |   |   +---handlers
        |   |   |   |   credential_ack_handler.py
        |   |   |   |   credential_issue_handler.py
        |   |   |   |   credential_offer_handler.py
        |   |   |   |   credential_problem_report_handler.py
        |   |   |   |   credential_proposal_handler.py
        |   |   |   |   credential_request_handler.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_credential_ack_handler.py
        |   |   |           test_credential_issue_handler.py
        |   |   |           test_credential_offer_handler.py
        |   |   |           test_credential_problem_report_handler.py
        |   |   |           test_credential_proposal_handler.py
        |   |   |           test_credential_request_handler.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---messages
        |   |   |   |   credential_ack.py
        |   |   |   |   credential_exchange_webhook.py
        |   |   |   |   credential_issue.py
        |   |   |   |   credential_offer.py
        |   |   |   |   credential_problem_report.py
        |   |   |   |   credential_proposal.py
        |   |   |   |   credential_request.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   +---inner
        |   |   |   |   |   credential_preview.py
        |   |   |   |   |   __init__.py
        |   |   |   |   |
        |   |   |   |   \---tests
        |   |   |   |           test_credential_preview.py
        |   |   |   |           __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_credential_ack.py
        |   |   |           test_credential_issue.py
        |   |   |           test_credential_offer.py
        |   |   |           test_credential_problem_report.py
        |   |   |           test_credential_proposal.py
        |   |   |           test_credential_request.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---models
        |   |   |   |   credential_exchange.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_credential_exchange.py
        |   |   |           __init__.py
        |   |   |
        |   |   \---tests
        |   |           test_manager.py
        |   |           test_routes.py
        |   |           __init__.py
        |   |
        |   \---v2_0
        |       |   controller.py
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---formats
        |       |   |   handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   +---indy
        |       |   |   |   handler.py
        |       |   |   |   __init__.py
        |       |   |   |
        |       |   |   \---tests
        |       |   |           test_handler.py
        |       |   |           __init__.py
        |       |   |
        |       |   \---ld_proof
        |       |       |   handler.py
        |       |       |   __init__.py
        |       |       |
        |       |       +---models
        |       |       |   |   cred_detail.py
        |       |       |   |   cred_detail_options.py
        |       |       |   |   __init__.py
        |       |       |   |
        |       |       |   \---tests
        |       |       |           test_cred_detail.py
        |       |       |           __init__.py
        |       |       |
        |       |       \---tests
        |       |               test_handler.py
        |       |               __init__.py
        |       |
        |       +---handlers
        |       |   |   cred_ack_handler.py
        |       |   |   cred_issue_handler.py
        |       |   |   cred_offer_handler.py
        |       |   |   cred_problem_report_handler.py
        |       |   |   cred_proposal_handler.py
        |       |   |   cred_request_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_cred_ack_handler.py
        |       |           test_cred_issue_handler.py
        |       |           test_cred_offer_handler.py
        |       |           test_cred_problem_report_handler.py
        |       |           test_cred_proposal_handler.py
        |       |           test_cred_request_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   cred_ack.py
        |       |   |   cred_ex_record_webhook.py
        |       |   |   cred_format.py
        |       |   |   cred_issue.py
        |       |   |   cred_offer.py
        |       |   |   cred_problem_report.py
        |       |   |   cred_proposal.py
        |       |   |   cred_request.py
        |       |   |   __init__.py
        |       |   |
        |       |   +---inner
        |       |   |   |   cred_preview.py
        |       |   |   |   __init__.py
        |       |   |   |
        |       |   |   \---tests
        |       |   |           test_cred_preview.py
        |       |   |           __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_cred_ack.py
        |       |           test_cred_format.py
        |       |           test_cred_issue.py
        |       |           test_cred_offer.py
        |       |           test_cred_problem_report.py
        |       |           test_cred_proposal.py
        |       |           test_cred_request.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |   |   cred_ex_record.py
        |       |   |   __init__.py
        |       |   |
        |       |   +---detail
        |       |   |   |   indy.py
        |       |   |   |   ld_proof.py
        |       |   |   |   __init__.py
        |       |   |   |
        |       |   |   \---tests
        |       |   |           test_indy.py
        |       |   |           test_ld_proof.py
        |       |   |           __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_cred_ex_record.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---notification
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   message_types.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   ack_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_ack_handler.py
        |       |           __init__.py
        |       |
        |       \---messages
        |               ack.py
        |               __init__.py
        |
        +---out_of_band
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   controller.py
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   problem_report_handler.py
        |       |   |   reuse_accept_handler.py
        |       |   |   reuse_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_problem_report_handler.py
        |       |           test_reuse_accept_handler.py
        |       |           test_reuse_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   invitation.py
        |       |   |   problem_report.py
        |       |   |   reuse.py
        |       |   |   reuse_accept.py
        |       |   |   service.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_invitation.py
        |       |           test_problem_report.py
        |       |           test_reuse.py
        |       |           test_reuse_accept.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |   |   invitation.py
        |       |   |   oob_record.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_invitation.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---present_proof
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   +---dif
        |   |   |   pres_exch.py
        |   |   |   pres_exch_handler.py
        |   |   |   pres_proposal_schema.py
        |   |   |   pres_request_schema.py
        |   |   |   pres_schema.py
        |   |   |   __init__.py
        |   |   |
        |   |   \---tests
        |   |           test_data.py
        |   |           test_pres_exch.py
        |   |           test_pres_exch_handler.py
        |   |           test_pres_request.py
        |   |           __init__.py
        |   |
        |   +---indy
        |   |       pres_exch_handler.py
        |   |       __init__.py
        |   |
        |   +---v1_0
        |   |   |   controller.py
        |   |   |   manager.py
        |   |   |   message_types.py
        |   |   |   routes.py
        |   |   |   __init__.py
        |   |   |
        |   |   +---handlers
        |   |   |   |   presentation_ack_handler.py
        |   |   |   |   presentation_handler.py
        |   |   |   |   presentation_problem_report_handler.py
        |   |   |   |   presentation_proposal_handler.py
        |   |   |   |   presentation_request_handler.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_presentation_ack_handler.py
        |   |   |           test_presentation_handler.py
        |   |   |           test_presentation_problem_report_handler.py
        |   |   |           test_presentation_proposal_handler.py
        |   |   |           test_presentation_request_handler.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---messages
        |   |   |   |   presentation.py
        |   |   |   |   presentation_ack.py
        |   |   |   |   presentation_problem_report.py
        |   |   |   |   presentation_proposal.py
        |   |   |   |   presentation_request.py
        |   |   |   |   presentation_webhook.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_presentation.py
        |   |   |           test_presentation_ack.py
        |   |   |           test_presentation_problem_report.py
        |   |   |           test_presentation_proposal.py
        |   |   |           test_presentation_request.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---models
        |   |   |   |   presentation_exchange.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_record.py
        |   |   |           __init__.py
        |   |   |
        |   |   \---tests
        |   |           test_manager.py
        |   |           test_routes.py
        |   |           __init__.py
        |   |
        |   \---v2_0
        |       |   controller.py
        |       |   manager.py
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---formats
        |       |   |   handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   +---dif
        |       |   |   |   handler.py
        |       |   |   |   __init__.py
        |       |   |   |
        |       |   |   \---tests
        |       |   |           test_handler.py
        |       |   |           __init__.py
        |       |   |
        |       |   \---indy
        |       |           handler.py
        |       |           __init__.py
        |       |
        |       +---handlers
        |       |   |   pres_ack_handler.py
        |       |   |   pres_handler.py
        |       |   |   pres_problem_report_handler.py
        |       |   |   pres_proposal_handler.py
        |       |   |   pres_request_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_pres_ack_handler.py
        |       |           test_pres_handler.py
        |       |           test_pres_problem_report_handler.py
        |       |           test_pres_proposal_handler.py
        |       |           test_pres_request_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   pres.py
        |       |   |   pres_ack.py
        |       |   |   pres_format.py
        |       |   |   pres_problem_report.py
        |       |   |   pres_proposal.py
        |       |   |   pres_request.py
        |       |   |   pres_webhook.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_pres.py
        |       |           test_pres_ack.py
        |       |           test_pres_format.py
        |       |           test_pres_problem_report.py
        |       |           test_pres_proposal.py
        |       |           test_pres_request.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |   |   pres_exchange.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_record.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_manager.py
        |               test_routes.py
        |               __init__.py
        |
        +---problem_report
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   handler.py
        |       |   message.py
        |       |   message_types.py
        |       |   __init__.py
        |       |
        |       \---tests
        |               test_handler.py
        |               test_message.py
        |               __init__.py
        |
        +---revocation_notification
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   +---v1_0
        |   |   |   message_types.py
        |   |   |   routes.py
        |   |   |   __init__.py
        |   |   |
        |   |   +---handlers
        |   |   |   |   revoke_handler.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_revoke_handler.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---messages
        |   |   |   |   revoke.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_revoke.py
        |   |   |           __init__.py
        |   |   |
        |   |   +---models
        |   |   |   |   rev_notification_record.py
        |   |   |   |   __init__.py
        |   |   |   |
        |   |   |   \---tests
        |   |   |           test_rev_notification_record.py
        |   |   |           __init__.py
        |   |   |
        |   |   \---tests
        |   |           test_routes.py
        |   |           __init__.py
        |   |
        |   \---v2_0
        |       |   message_types.py
        |       |   routes.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   revoke_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_revoke_handler.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   revoke.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_revoke.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |   |   rev_notification_record.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_rev_notification_record.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_routes.py
        |               __init__.py
        |
        +---routing
        |   |   definition.py
        |   |   __init__.py
        |   |
        |   \---v1_0
        |       |   manager.py
        |       |   message_types.py
        |       |   __init__.py
        |       |
        |       +---handlers
        |       |   |   forward_handler.py
        |       |   |   route_query_request_handler.py
        |       |   |   route_query_response_handler.py
        |       |   |   route_update_request_handler.py
        |       |   |   route_update_response_handler.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_forward_handler.py
        |       |           test_query_update_handlers.py
        |       |           __init__.py
        |       |
        |       +---messages
        |       |   |   forward.py
        |       |   |   route_query_request.py
        |       |   |   route_query_response.py
        |       |   |   route_update_request.py
        |       |   |   route_update_response.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_forward.py
        |       |           test_route_query_request.py
        |       |           test_route_query_response.py
        |       |           test_route_update_request.py
        |       |           test_route_update_response.py
        |       |           __init__.py
        |       |
        |       +---models
        |       |   |   paginate.py
        |       |   |   paginated.py
        |       |   |   route_query_result.py
        |       |   |   route_record.py
        |       |   |   route_update.py
        |       |   |   route_updated.py
        |       |   |   __init__.py
        |       |   |
        |       |   \---tests
        |       |           test_route_record.py
        |       |           __init__.py
        |       |
        |       \---tests
        |               test_routing_manager.py
        |               __init__.py
        |
        +---tests
        |       test_didcomm_prefix.py
        |       __init__.py
        |
        \---trustping
        |   definition.py
        |   __init__.py
        |
        \---v1_0
                |   message_types.py
                |   routes.py
                |   __init__.py
                |
                +---handlers
                |   |   ping_handler.py
                |   |   ping_response_handler.py
                |   |   __init__.py
                |   |
                |   \---tests
                |           test_ping_handler.py
                |           test_ping_response_handler.py
                |           __init__.py
                |
                +---messages
                |   |   ping.py
                |   |   ping_response.py
                |   |   __init__.py
                |   |
                |   \---tests
                |           test_trust_ping.py
                |           test_trust_ping_reponse.py
                |           __init__.py
                |
                \---tests
                        test_routes.py
                        __init__.py